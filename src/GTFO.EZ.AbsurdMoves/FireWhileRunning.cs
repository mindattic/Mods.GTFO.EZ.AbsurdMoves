using BepInEx.Configuration;
using Gear;
using HarmonyLib;
using UnityEngine;

namespace GTFO.EZ.AbsurdMoves;

// The game's gun logic (BulletWeaponArchetype.Update) drops the trigger whenever its owner's
// locomotion says it's running. For the local player's own gun, the running flag reads false for
// the length of that one call, then goes straight back - in a finalizer, so it goes back even if
// the call throws, and the sprint itself carries on.
[HarmonyPatch(typeof(BulletWeaponArchetype), nameof(BulletWeaponArchetype.Update))]
internal static class FireWhileRunning
{
    internal static ConfigEntry<bool> Enabled = null!;

    internal static void Bind(ConfigFile config)
    {
        Enabled = config.Bind("Fire On The Move", "Fire While Running Enabled", true,
            "Turns firing while sprinting on or off. On: your guns fire while you sprint, without slowing you down. " +
            "Off: vanilla (the trigger does nothing while sprinting).");
    }

    [HarmonyPrefix]
    private static void Prefix(BulletWeaponArchetype __instance, out bool __state)
    {
        __state = false;
        if (!MovesToggle.Active || !Enabled.Value) return;
        var loc = FireGate.LocalLocomotion(__instance);
        if (loc == null || !loc.IsRunning) return;
        __state = true;
        loc.IsRunning = false;
    }

    [HarmonyFinalizer]
    private static void Finalizer(BulletWeaponArchetype __instance, bool __state)
    {
        if (!__state) return;
        var loc = __instance.m_owner?.Locomotion;
        if (loc != null) loc.IsRunning = true;
    }
}

// The first-person gun sits in its lowered sprint pose for as long as the item holder's own
// running flag is set (the run state sets it on entry and clears it on exit). While you're pulling
// the trigger mid-sprint, and for a moment after, that flag reads false so the gun comes up level
// and the shots, tracers and muzzle flash leave a barrel pointed where you're aiming. It's set back
// every frame from the real run state, so stopping or leaving the sprint behaves exactly as vanilla.
// Teammates' view needs nothing: the sprint's synced state keeps the gun's aim IK on.
[HarmonyPatch(typeof(FirstPersonItemHolder), nameof(FirstPersonItemHolder.Update))]
internal static class FireWhileRunningPose
{
    private const float RaisedFor = 0.6f;
    private const float RaiseRate = 6f;

    private static float _lastTrigger = -10f;
    // Whether the last frame left the holder's running flag cleared mid-sprint: switching the
    // feature (or Absurd Moves) off then has to set it back, since the run state only sets it on entry.
    private static bool _raised;

    [HarmonyPrefix]
    private static void Prefix(FirstPersonItemHolder __instance)
    {
        try
        {
            var owner = __instance.m_owner;
            if (owner == null || !owner.IsLocallyOwned || owner.Owner == null || owner.Owner.IsBot) return;
            var loc = owner.Locomotion;
            var running = loc != null && loc.m_currentStateEnum == Player.PlayerLocomotion.PLOC_State.Run;
            if (!MovesToggle.Active || !FireWhileRunning.Enabled.Value)
            {
                if (_raised && running) __instance.IsRunning = true;
                _raised = false;
                return;
            }
            if (!running)
            {
                _raised = false; // leaving the run state cleared the flag itself
                return;
            }

            var gun = __instance.WieldedItem?.TryCast<BulletWeapon>() != null;
            if (gun && Moves.InPlay && InputMapper.GetButton.Invoke(InputAction.Fire, eFocusState.FPS)) _lastTrigger = Time.time;
            var raised = gun && Time.time - _lastTrigger < RaisedFor;

            __instance.IsRunning = !raised;
            _raised = raised;
            if (!raised) return;
            __instance.RunInertia = 0f;
            __instance.m_runWeight = Mathf.MoveTowards(__instance.m_runWeight, 0f, Time.deltaTime * RaiseRate);
        }
        catch (Exception e)
        {
            Plugin.Logger.LogWarning($"GTFO.EZ.AbsurdMoves sprint-fire pose failed: {e.Message}");
        }
    }
}
