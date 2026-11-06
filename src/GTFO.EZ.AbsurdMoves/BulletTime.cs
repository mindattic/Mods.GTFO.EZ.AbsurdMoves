using BepInEx.Configuration;
using GTFO.API;
using MindAttic.GTFO.Shared;
using Player;
using UnityEngine;

namespace GTFO.EZ.AbsurdMoves;

/// <summary>
/// Max Payne bullet time: a tap of the key slows the whole game clock (Time.timeScale) - everyone and
/// everything, players, bots, enemies and projectiles alike - easing down into it, holding, then
/// easing back up to full speed when it's over. Its own timing runs on real time, so the duration
/// and cooldown don't stretch while time is slowed. The world's sound drops to a hollow, muffled mix
/// with a deep boom going in and a heartbeat under it. A press while it's running or cooling down
/// does nothing. The trigger is broadcast, carrying the triggerer's own duration and time scale, so
/// every player's game slows together, by the same amount for the same time, and everyone's event log
/// says who triggered it; once the cooldown is over, the event log says it's ready again.
/// </summary>
internal sealed class BulletTime : MonoBehaviour
{
    // v2: the message now carries the duration and time scale; a new name so an older install never
    // reads the bigger message as the old empty one (or the other way round).
    private const string StartEvent = "MindAttic.GTFO.EZ.AbsurdMoves.BulletTime.v2";
    private const float RampIn = 0.4f;
    private const float RampOut = 0.8f;

    private static ConfigEntry<bool> featureOn = null!;
    private static ConfigEntry<KeyCode> key = null!;
    private static ConfigEntry<float> duration = null!;
    private static ConfigEntry<float> cooldown = null!;
    private static ConfigEntry<float> timeScale = null!;
    private static ConfigEntry<string> muffle = null!;
    private static ConfigEntry<string> enterSound = null!;
    private static ConfigEntry<string> exitSound = null!;
    private static ConfigEntry<string> loopStart = null!;
    private static ConfigEntry<string> loopStop = null!;

    private static readonly List<(uint Id, float Target, float Before)> rtpcs = new();
    // Real time (Time.unscaledTime), never game time: the game clock is what bullet time slows.
    private static float startedAt = -1f;
    private static float readyAt;
    // The physics step and time scale as they were before bullet time, put back after it.
    private static float baseFixedDelta = -1f, baseTimeScale = 1f;
    // The running bullet time's length and time scale: the triggerer's settings, not this machine's.
    private static float activeDuration = 5f, activeScale = 0.3f;
    // Set when bullet time ends; the event log says so once the cooldown is over.
    private static bool announceReady;

    public BulletTime(IntPtr ptr) : base(ptr) { }

    internal static void Bind(ConfigFile config)
    {
        const string s = "Bullet Time";
        featureOn = config.Bind(s, "Enabled", true,
            "Turns bullet time on or off. On: tap the key to slow the whole game - everyone and everything - for a few seconds, Max Payne style.");
        key = config.Bind(s, "Key", KeyCode.LeftAlt,
            "Starts bullet time as you let go of it. A press with another key in it (Alt+Tab, Alt+Enter) or one that loses the game focus doesn't count.");
        duration = config.Bind(s, "DurationSeconds", 5f, "How long bullet time lasts, in real seconds.");
        cooldown = config.Bind(s, "CooldownSeconds", 10f, "Real seconds after bullet time ends before it can start again.");
        timeScale = config.Bind(s, "TimeScale", 0.3f, "How fast the game runs during bullet time (1 = normal, 0.3 = under a third).");
        muffle = config.Bind(s, "SoundMuffle", "SOUND_EFFECT_MUFFLE=100, GAME_MENU_MUFFLE=100",
            "The game's Wwise mix parameters (NAME=value, comma-separated) pushed during bullet time for the hollow, underwater sound; each goes back to what it was after. Empty = no muffle.");
        enterSound = config.Bind(s, "EnterSound", "STINGER_LOW_BOOM", "Played as bullet time starts (a deep boom). Empty = silent.");
        exitSound = config.Bind(s, "ExitSound", "COMBAT_FADING_PULSE_LEVEL_1", "Played as bullet time ends. Empty = silent.");
        loopStart = config.Bind(s, "LoopSound", "ENEMYHEARTBEATLOOP", "A loop under bullet time (a slow heartbeat). Empty = none.");
        loopStop = config.Bind(s, "LoopStopSound", "ENEMYHEARTBEATLOOPEND", "Stops the loop.");
    }

    // A teammate's trigger always starts it here, even if this machine is still cooling down from its
    // own (clocks and cooldown settings differ between players): otherwise one game would run at full
    // speed while the others are slowed.
    // The handler runs inside GTFO-API's prefix on the game's packet receive, so nothing escapes it
    // (see GroundPound.Init); a start that fails halfway is ended so nothing stays slowed or muffled.
    internal static void Init() => RemoteEvents.Register<Trigger>(StartEvent, (sender, trigger) =>
    {
        try
        {
            if (Begin(trigger.Duration, trigger.TimeScale, fromTeammate: true)) AnnounceTrigger(sender);
        }
        catch (Exception e)
        {
            Plugin.Logger.LogError($"GTFO.EZ.AbsurdMoves teammate's bullet time failed: {e}");
            try
            {
                if (Active) End();
            }
            catch (Exception) { }
        }
    });

    private static bool Active => startedAt >= 0f;

    // The Absurd Moves toggle turning off while bullet time is running: snaps the clock back to
    // normal right away instead of leaving the game slowed with the toggle supposedly off.
    internal static void ForceEnd()
    {
        if (Active) End();
    }

    private void Update()
    {
        try
        {
            if (GameStateManager.CurrentStateName != eGameStateName.InLevel)
            {
                if (Active) End();
                readyAt = 0f;
                armed = false;
                announceReady = false;
                return;
            }

            if (KeyTapped() && Begin(duration.Value, timeScale.Value))
            {
                NetworkAPI.InvokeEvent(StartEvent, new Trigger(activeDuration, activeScale));
                AnnounceTrigger(SNetwork.SNet.LocalPlayer != null ? SNetwork.SNet.LocalPlayer.Lookup : 0UL);
            }

            if (Active) Tick();
            else if (announceReady && Time.unscaledTime >= readyAt)
            {
                announceReady = false;
                if (featureOn.Value) MessageHelper.Show("Bullet time ready", MessageType.EventLog);
            }
        }
        catch (Exception e)
        {
            Plugin.Logger.LogError($"GTFO.EZ.AbsurdMoves bullet time failed: {e}");
            if (Active) End();
        }
    }

    // Never leave the game slowed: quitting or the object going away puts the clock back.
    private void OnDestroy()
    {
        if (Active) RestoreClock();
    }

    // The key counts on release, and only if nothing else was pressed while it was held and the game
    // kept focus: the default Left Alt is also the Alt of Alt+Tab, and the Alt press reaches the game
    // before the window loses focus, so a press-to-start would fire on every Alt+Tab.
    private static bool armed;

    private static bool KeyTapped()
    {
        if (!MovesToggle.Active || !featureOn.Value || FocusStateManager.CurrentState != eFocusState.FPS || !Application.isFocused)
        {
            armed = false;
            return false;
        }
        if (Input.GetKeyDown(key.Value)) armed = true;
        else if (armed && Input.anyKeyDown) armed = false; // a chord: Alt+Tab, Alt+F4, Alt+Enter
        if (!armed || !Input.GetKeyUp(key.Value)) return false;
        armed = false;
        return true;
    }

    private void OnApplicationFocus(bool focused)
    {
        if (!focused) armed = false;
    }

    // "Bullet time - triggered by Bob" in everyone's event log, the triggerer's own included.
    private static void AnnounceTrigger(ulong lookup)
    {
        var text = $"Bullet time - triggered by {MovesToggle.PlayerName(lookup)}";
        if (!MessageHelper.Show(text, MessageType.EventLog)) Plugin.Logger.LogInfo($"GTFO.EZ.AbsurdMoves: {text}");
    }

    // The duration and time scale are the triggerer's (sanitised, since a teammate's come off the
    // network): every machine runs the same bullet time whatever its own config says.
    private static bool Begin(float seconds, float scale, bool fromTeammate = false)
    {
        if (Active || (!fromTeammate && Time.unscaledTime < readyAt)) return false;
        activeDuration = float.IsFinite(seconds) ? Mathf.Clamp(seconds, RampIn + RampOut, 60f) : RampIn + RampOut;
        activeScale = float.IsFinite(scale) ? Mathf.Clamp(scale, 0.05f, 1f) : 1f;
        startedAt = Time.unscaledTime;
        baseTimeScale = Time.timeScale > 0f ? Time.timeScale : 1f;
        baseFixedDelta = Time.fixedDeltaTime;
        rtpcs.Clear();
        foreach (var pair in muffle.Value.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            var parts = pair.Split('=', 2, StringSplitOptions.TrimEntries);
            if (parts.Length != 2 || !float.TryParse(parts[1], System.Globalization.NumberStyles.Float,
                    System.Globalization.CultureInfo.InvariantCulture, out var value)) continue;
            var id = AkSoundEngine.GetIDFromString(parts[0]);
            rtpcs.Add((id, value, CellSound.GetGlobalRTPCValue(id)));
        }
        if (PlayerManager.GetLocalPlayerAgent() is { } player) Sounds.Play(enterSound.Value, player.Position);
        return true;
    }

    // 0 -> 1 easing down over the ramp-in, 1 for the body, back to 0 easing up over the ramp-out.
    private static float Strength(float elapsed)
    {
        var total = activeDuration;
        if (elapsed < RampIn) return Mathf.SmoothStep(0f, 1f, elapsed / RampIn);
        if (elapsed > total - RampOut) return Mathf.SmoothStep(0f, 1f, (total - elapsed) / RampOut);
        return 1f;
    }

    private static void Tick()
    {
        var elapsed = Time.unscaledTime - startedAt;
        if (elapsed >= activeDuration)
        {
            End();
            return;
        }
        var strength = Strength(elapsed);

        // The whole game clock, with the physics step scaled along so movement stays smooth.
        var scale = baseTimeScale * Mathf.Lerp(1f, activeScale, strength);
        Time.timeScale = scale;
        Time.fixedDeltaTime = baseFixedDelta * scale / baseTimeScale;

        foreach (var (id, target, before) in rtpcs) CellSound.SetGlobalRTPCValue(id, Mathf.Lerp(before, target, strength));

        var player = PlayerManager.GetLocalPlayerAgent();
        if (player != null) LoopSounds.Touch("bullet-time", loopStart.Value, loopStop.Value, player.Position);
    }

    private static void RestoreClock()
    {
        Time.timeScale = baseTimeScale;
        if (baseFixedDelta > 0f) Time.fixedDeltaTime = baseFixedDelta;
    }

    private static void End()
    {
        RestoreClock();
        foreach (var (id, _, before) in rtpcs) CellSound.SetGlobalRTPCValue(id, before);
        rtpcs.Clear();
        startedAt = -1f;
        readyAt = Time.unscaledTime + cooldown.Value;
        announceReady = true;
        if (GameStateManager.CurrentStateName == eGameStateName.InLevel && PlayerManager.GetLocalPlayerAgent() is { } player)
            Sounds.Play(exitSound.Value, player.Position);
    }

    // GTFO-API sends structs as raw bytes: the triggerer's duration (real seconds) and time scale.
    private readonly struct Trigger
    {
        public readonly float Duration, TimeScale;

        public Trigger(float duration, float timeScale) => (Duration, TimeScale) = (duration, timeScale);
    }
}
