using UnityEngine;

namespace GTFO.EZ.AbsurdMoves;

internal static class Sounds
{
    private static readonly HashSet<string> failed = new();

    internal static void Play(string eventName, Vector3 position)
    {
        if (string.IsNullOrWhiteSpace(eventName)) return;
        try
        {
            CellSound.Post(eventName.Trim(), position);
        }
        catch (Exception e)
        {
            if (failed.Add(eventName)) Plugin.Logger.LogWarning($"GTFO.EZ.AbsurdMoves couldn't play sound '{eventName}': {e.Message}");
        }
    }
}

/// <summary>
/// Looping sounds that run while something keeps "touching" them and stop a moment after the
/// touches stop. One per key, each on its own sound player so it can be moved and stopped.
/// </summary>
internal sealed class LoopSounds : MonoBehaviour
{
    private const float StopAfter = 0.25f;

    private sealed class Loop
    {
        public CellSoundPlayer Player = null!;
        public string StopEvent = "";
        public Vector3 Position;
        public float LastTouch;
    }

    private static readonly Dictionary<string, Loop> loops = new();
    private static readonly HashSet<string> failed = new();

    public LoopSounds(IntPtr ptr) : base(ptr) { }

    internal static void Touch(string key, string startEvent, string stopEvent, Vector3 position)
    {
        if (string.IsNullOrWhiteSpace(startEvent) || failed.Contains(startEvent)) return;
        CellSoundPlayer? created = null;
        try
        {
            if (!loops.TryGetValue(key, out var loop))
            {
                created = new CellSoundPlayer(position);
                loop = new Loop { Player = created, StopEvent = stopEvent };
                loop.Player.Post(startEvent.Trim(), position);
                loops[key] = loop;
            }
            else
            {
                loop.Player.UpdatePosition(position);
            }
            loop.Position = position;
            loop.LastTouch = Time.time;
        }
        catch (Exception e)
        {
            // Touched every frame: a sound that can't play is given up on, not retried (and leaked) each frame.
            if (failed.Add(startEvent)) Plugin.Logger.LogWarning($"GTFO.EZ.AbsurdMoves couldn't run loop sound '{startEvent}': {e.Message}");
            // An existing loop that failed to update is dropped too: stop it, or it plays on forever.
            if (created == null && loops.Remove(key, out var orphan))
            {
                try { orphan.Player.Stop(); orphan.Player.Recycle(); } catch { }
            }
            else loops.Remove(key);
            try { created?.Recycle(); } catch { }
        }
    }

    private void Update()
    {
        if (loops.Count == 0) return;
        List<string>? stale = null;
        foreach (var (key, loop) in loops)
            if (Time.time - loop.LastTouch > StopAfter) (stale ??= new List<string>()).Add(key);
        if (stale == null) return;
        foreach (var key in stale)
        {
            var loop = loops[key];
            loops.Remove(key);
            try
            {
                if (!string.IsNullOrWhiteSpace(loop.StopEvent)) loop.Player.Post(loop.StopEvent.Trim(), loop.Position);
                else loop.Player.Stop();
                loop.Player.Recycle();
            }
            catch (Exception e)
            {
                Plugin.Logger.LogWarning($"GTFO.EZ.AbsurdMoves couldn't stop a loop sound: {e.Message}");
            }
        }
    }
}
