using System;
using System.Collections.Generic;
using BepInEx.Configuration;

namespace GTFO.EZ.AbsurdMoves;

/// <summary>
/// G.EZ-T145: the host's values for this mod's move settings (every number and switch outside the
/// per-player sections: which moves are on, ground pound's shockwave, bullet time...), while you're in
/// someone else's lobby. Every machine runs the moves from its own settings, so a team must play one
/// set. While host values are held they replace the matching entries' values in memory - never in your
/// .cfg, which is not saved while they do; leaving the lobby puts your own values back.
///
/// GTFO.EZ fills this (by reflection, on this public type) from the host values its level-settings
/// sync already receives. Without GTFO.EZ nothing ever sets it, and your own values are used.
/// </summary>
public static class HostLevelSettings
{
    private static readonly Dictionary<ConfigEntryBase, object> Own = new();
    private static ConfigFile? _config;

    /// <summary>Whether host values are held (in someone else's lobby, after the host sent them).</summary>
    public static bool Active { get; private set; }

    internal static void Init(ConfigFile config) => _config = config;

    // The sections that are yours alone: the master switch and the migration marker.
    private static bool IsShared(ConfigDefinition d) => d.Section is not ("0. Mod" or "Internal");

    /// <summary>
    /// The host's values, keyed "Section/Key" as in this mod's .cfg (a bool as 1/0). A setting the
    /// host didn't send keeps your own value. Null or empty clears.
    /// </summary>
    public static void Set(IDictionary<string, float>? values)
    {
        if (values == null || values.Count == 0)
        {
            Clear();
            return;
        }
        if (_config == null) return;
        var applied = Swap(entry => values.TryGetValue($"{entry.Definition.Section}/{entry.Definition.Key}", out var host) ? host : null);
        Active = true;
        Plugin.Logger.LogInfo($"Level settings: using the host's values for {applied} GTFO.EZ.AbsurdMoves setting(s).");
    }

    /// <summary>Back to your own values (left the lobby, or hosting yourself).</summary>
    public static void Clear()
    {
        if (!Active) return;
        Swap(_ => null);
        Own.Clear();
        Active = false;
        Plugin.Logger.LogInfo("Level settings: back to your own GTFO.EZ.AbsurdMoves settings.");
    }

    // Puts `host(entry)` into each shared numeric or bool entry that has one, and your own value back
    // into the rest. Returns how many entries hold a host value.
    private static int Swap(Func<ConfigEntryBase, float?> host)
    {
        var saveOnSet = _config!.SaveOnConfigSet;
        _config.SaveOnConfigSet = false;
        var applied = 0;
        try
        {
            foreach (var pair in _config)
            {
                var entry = pair.Value;
                if (!IsShared(pair.Key) || !(entry.SettingType == typeof(float) || entry.SettingType == typeof(int) || entry.SettingType == typeof(bool))) continue;
                var wanted = host(entry);
                if (wanted == null || float.IsNaN(wanted.Value))
                {
                    if (Own.Remove(entry, out var own)) entry.BoxedValue = own;
                    continue;
                }
                Own.TryAdd(entry, entry.BoxedValue);
                entry.BoxedValue = entry.SettingType == typeof(bool) ? wanted.Value != 0f
                    : entry.SettingType == typeof(int) ? (object)(int)MathF.Round(wanted.Value)
                    : wanted.Value;
                applied++;
            }
        }
        finally
        {
            _config.SaveOnConfigSet = saveOnSet;
        }
        return applied;
    }
}
