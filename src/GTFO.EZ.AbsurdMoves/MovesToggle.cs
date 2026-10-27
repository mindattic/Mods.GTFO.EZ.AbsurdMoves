using BepInEx.Configuration;
using MindAttic.GTFO.Shared;
using Player;
using UnityEngine;

namespace GTFO.EZ.AbsurdMoves;

/// <summary>
/// Absurd Moves as a whole is a toggle (<see cref="key"/>, Home by default), off at the start of
/// every level. The first press turns it on, so the dodge, bullet time, fire mid-air/while running,
/// ground pound, mantle, double jump, slide and high jump all start working; the second press turns
/// it off again - cancelling whatever's in progress (a pound, a mantle, a slide, a charged high
/// jump, an in-flight dodge, a running bullet time) and letting none of them start again until it's
/// back on.
/// </summary>
internal static class MovesToggle
{
    private static ConfigEntry<KeyCode> key = null!;

    internal static bool Active { get; private set; }

    internal static void Bind(ConfigFile config)
    {
        key = config.Bind("Moves Toggle", "ToggleAbsurdMovesKey", KeyCode.Home,
            "Switches every Absurd Move on or off, all at once. Off: the dodge, bullet time, fire mid-air/while " +
            "running, ground pound, mantle, double jump, slide and high jump all do nothing, and whichever of " +
            "them is in progress is cancelled right away. Off at the start of every level.");
    }

    // Called once a frame, from Moves.Postfix, for the local player's own locomotion only.
    internal static void Tick(PlayerLocomotion loc)
    {
        if (!Moves.InPlay || !Input.GetKeyDown(key.Value)) return;

        Active = !Active;
        if (!Active) CancelAll(loc);
        Announce(Active ? "Absurd Moves: ON" : "Absurd Moves: OFF");
    }

    // The level ended, or the player died or left: back off, the same as every move's own state.
    internal static void ResetForLevel() => Active = false;

    private static void CancelAll(PlayerLocomotion loc)
    {
        GroundPound.Reset(loc);
        Mantle.Reset();
        AirMoves.Reset();
        Slide.Reset();
        HighJump.Reset();
        DodgeRunner.Cancel();
        BulletTime.ForceEnd();
    }

    // On the short line under the crosshair, in the event log, and always in the console too (not
    // just a fallback for when the on-screen message fails), so the Absurd Moves on/off state is
    // never just text that scrolled past - same as InfiniteToggles' infinite health/ammo announcements.
    private static void Announce(string message)
    {
        MessageHelper.Show(message, MessageType.Center, 3f);
        MessageHelper.Show(message, MessageType.EventLog);
        Plugin.Logger.LogInfo($"GTFO.EZ.AbsurdMoves: {message}");
    }
}
