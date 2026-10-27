using HarmonyLib;
using Player;
using UnityEngine;

namespace GTFO.EZ.AbsurdMoves;

// Shared per-frame driver and helpers for the movement features: runs once per frame for the
// local player's locomotion only (never a teammate's or a bot's).
[HarmonyPatch(typeof(PlayerLocomotion), nameof(PlayerLocomotion.Update))]
internal static class Moves
{
    internal static bool InPlay => FocusStateManager.CurrentState == eFocusState.FPS;

    internal static bool InAir(PlayerLocomotion loc) =>
        loc.m_currentStateEnum is PlayerLocomotion.PLOC_State.Jump or PlayerLocomotion.PLOC_State.Fall;

    internal static Vector3 FlatForward(PlayerAgent agent)
    {
        var forward = agent.FPSCamera != null ? agent.FPSCamera.FlatForward : agent.transform.forward;
        forward.y = 0f;
        return forward.sqrMagnitude > 0.0001f ? forward.normalized : Vector3.forward;
    }

    // Fall damage is measured from where the fall started, so a double jump moves that start
    // along with you: you only take damage for what you drop after it ends.
    internal static void ResetFallStart(PlayerAgent agent, PlayerLocomotion loc)
    {
        var fall = loc.CurrentState?.TryCast<PLOC_Fall>();
        if (fall != null) fall.m_startPos = agent.Position;
    }

    // The held gun's own jump bob, as a normal jump plays it.
    internal static void JumpSway(PlayerAgent agent) => agent.FPItemHolder?.PlayJumpAnimation();

    // A gap this long between updates means the level ended, or the player was dead or gone.
    private const float StaleAfter = 0.5f;

    private static IntPtr _lastLoc;
    private static float _lastTick = -10f;

    // Drops every move in progress: a mantle's lowered gun comes back up, a pound's fall-damage
    // immunity ends, and nothing half-finished carries into the next life or level.
    private static void ResetAll(PlayerLocomotion? loc)
    {
        GroundPound.Reset(loc);
        Mantle.Reset();
        AirMoves.Reset();
        Slide.Reset();
    }

    [HarmonyPostfix]
    private static void Postfix(PlayerLocomotion __instance)
    {
        try
        {
            if (!__instance.LocallyOwned) return;
            var agent = __instance.m_owner;
            if (agent == null || agent.Owner == null || agent.Owner.IsBot) return;
            if (!agent.Alive || GameStateManager.CurrentStateName != eGameStateName.InLevel)
            {
                ResetAll(__instance);
                MovesToggle.ResetForLevel();
                return;
            }

            if (__instance.Pointer != _lastLoc || Time.time - _lastTick > StaleAfter) ResetAll(__instance);
            _lastLoc = __instance.Pointer;
            _lastTick = Time.time;

            MovesToggle.Tick(__instance);

            GroundPound.Tick(agent, __instance);
            Mantle.Tick(agent, __instance);
            if (!Mantle.Active) AirMoves.Tick(agent, __instance);
            Slide.Tick(agent, __instance);
        }
        catch (Exception e)
        {
            Plugin.Logger.LogWarning($"GTFO.EZ.AbsurdMoves movement update failed: {e}");
        }
    }
}
