using System;
using BepInEx.Configuration;

namespace GTFO.EZ.AbsurdMoves;

/// <summary>
/// BepInEx keeps whatever value is already in an existing .cfg, so when this mod changes a default,
/// installs that already have the setting never pick up the new one. Each entry here moves a
/// setting still sitting on its old default over to its new one (read off the Bind call itself, so
/// the new value lives in one place), once - only for configs that haven't reached that entry's
/// version yet - so a value a player set deliberately is never overwritten. (The same scheme as
/// GTFO.EZ's and GTFO.EZ.AbsurdWeapons'.) Changing a default: add a line with the next version
/// number and the old default.
/// </summary>
internal static class ConfigMigrations
{
    private static readonly (int Version, string Section, string Key, object OldDefault)[] DefaultMigrations =
    {
        (1, "Slide", "Slide Duration (seconds)", 1.2f), // v14: twice the slide
        (2, "Moves Toggle", "ToggleAbsurdMovesKey", UnityEngine.KeyCode.End), // v16: End -> Home (AbsurdWeapons took End)
    };

    // After every setting is bound.
    internal static void Apply(ConfigFile config)
    {
        var version = config.Bind("Internal", "Config Migration Version", 0,
            "Internal bookkeeping - do not edit. Tracks which one-time default migrations have already run.");
        var reached = version.Value;
        var latest = reached;
        foreach (var (target, section, key, oldDefault) in DefaultMigrations)
        {
            latest = Math.Max(latest, target);
            if (reached >= target) continue;
            var definition = new ConfigDefinition(section, key);
            if (!config.ContainsKey(definition)) continue;
            var entry = config[definition];
            if (!Equals(entry.BoxedValue, oldDefault)) continue;
            entry.BoxedValue = entry.DefaultValue;
            Plugin.Logger.LogInfo($"GTFO.EZ.AbsurdMoves config: [{section}] {key} moved from its old default {oldDefault} to {entry.DefaultValue}.");
        }
        if (latest != reached) version.Value = latest;
    }
}
