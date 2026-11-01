using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using Il2CppInterop.Runtime.Injection;

namespace GTFO.EZ.AbsurdMoves;

[BepInPlugin(GUID, NAME, VERSION)]
[BepInProcess("GTFO.exe")]
[BepInDependency("dev.gtfomodding.gtfo-api")]
public class Plugin : BasePlugin
{
    public const string GUID = "MindAttic.GTFO.EZ.AbsurdMoves";
    public const string NAME = "GTFO.EZ.AbsurdMoves";
    public const string VERSION = "19.0.0";

    internal static ManualLogSource Logger = null!;

    public override void Load()
    {
        Logger = Log;

        // The master switch is the cfg's first entry ("0. Mod" sorts ahead of every other section), so
        // the mod can be turned off from its .cfg where a mod manager profile doesn't list it.
        var enabled = Config.Bind("0. Mod", "Enabled", true,
            "Turn the whole mod on or off. When false, the mod loads nothing: no patches, no moves, no hotkeys.");
        if (!enabled.Value)
        {
            Logger.LogInfo($"{NAME} is switched off in its config ([0. Mod] Enabled) - not loading.");
            return;
        }

        MovesToggle.Bind(Config);
        Dodge.Bind(Config);
        BulletTime.Bind(Config);
        AirMoves.Bind(Config);
        Slide.Bind(Config);
        FireMidAir.Bind(Config);
        FireWhileRunning.Bind(Config);
        GroundPound.Bind(Config);
        Mantle.Bind(Config);
        ConfigMigrations.Apply(Config); // after every Bind: stale old defaults move to the new ones

        ClassInjector.RegisterTypeInIl2Cpp<LoopSounds>();
        ClassInjector.RegisterTypeInIl2Cpp<BulletTime>();
        ClassInjector.RegisterTypeInIl2Cpp<PoundPushes>();
        BulletTime.Init();
        GroundPound.Init();

        new Harmony(GUID).PatchAll();
        AddComponent<LoopSounds>();
        AddComponent<BulletTime>();
        AddComponent<PoundPushes>();

        Logger.LogInfo($"{NAME} v{VERSION} loaded.");
    }
}
