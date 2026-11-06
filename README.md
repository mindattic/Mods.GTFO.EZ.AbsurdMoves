# GTFO.EZ.AbsurdMoves

![GTFO.EZ.AbsurdMoves](logo.png)

*(Listed on Thunderstore as `GtfoEZAbsurdMoves`. Package names there can only use letters, numbers
and underscores, so they can't contain a period.)*

GTFO.EZ.AbsurdMoves is a GTFO.EZ companion mod, built on [GTFO.EZ](https://github.com/mindattic/GTFO.EZ)'s core: it carries GTFO.EZ's shared code (the Insert/Delete infinite health and ammo toggles, and MessageHelper, which puts on-screen messages wherever they fit) and works with or without GTFO.EZ installed. It was called GTFO.AbsurdMoves before its first release.

A BepInEx plugin for GTFO that adds new player moves: double jump, ground
pound, mantling, slide, dodge and bullet time, plus firing on the move.
No game files are shipped or edited.

**Every move has its own on/off switch, and its numbers can be retuned,** in the config file
BepInEx creates on first launch: `BepInEx\config\MindAttic.GTFO.EZ.AbsurdMoves.cfg`.

### `[0. Mod]`

The file's first entry switches the whole mod off, for a mod manager profile that doesn't list it:

| Setting | Default | What it does |
|---|---|---|
| `Enabled` | true | Turn the whole mod on or off. When false, the mod loads nothing: no patches, no moves, no hotkeys. Takes effect on restart. |

## Absurd Moves toggle

Every move below is off at the start of a level. **Press Home** to turn Absurd Moves on; press it
again to turn it off. Turning it off cancels whatever's in progress right away - an in-flight dodge
stops, a running bullet time snaps back to normal speed, a pound/mantle/slide resets - and none of
them start again until Home is pressed again. The state (and whichever move was cancelled) is
announced on-screen, in the event log and in the console - same as the Insert/Delete infinite
health and ammo toggles.

| Setting | Default | What it does |
|---|---|---|
| `[Moves Toggle] ToggleAbsurdMovesKey` | Home | Switches every Absurd Move on or off, all at once. Off at the start of every level. |

## The moves

| Move | How | Switch |
|---|---|---|
| [Double jump](#double-jump) | In the air, press **Jump** again | `[Double Jump] Double Jump Enabled` |
| [Ground pound](#ground-pound) | In the air, high enough up, press **Crouch** | `[Ground Pound] Ground Pound Enabled` |
| [Mantling](#mantling) | Jump at a ledge no higher than your eyes, holding **forward** | `[Mantling] Mantling Enabled` |
| [Slide](#slide) | Press **Crouch** while sprinting | `[Slide] Slide Enabled` |
| [Dodge](#dodge) | Hold **Shift** and double-tap **W/A/S/D** | `[Dodge] Dodge Enabled` |
| [Bullet time](#bullet-time) | Tap **Left Alt** | `[Bullet Time] Enabled` |
| [Infinite ammo / health](#infinite-ammo-and-health) | Press **Insert** / **Delete** | Toggles, off at the start |
| [Fire mid-air](#fire-on-the-move) | Shoot while jumping or falling | `[Fire On The Move] Fire Mid-Air Enabled` |
| [Fire while running](#fire-on-the-move) | Shoot while sprinting | `[Fire On The Move] Fire While Running Enabled` |

Jump and Crouch are the game's own bindings, so they follow whatever you've rebound them to. Every
move above (not Insert/Delete, which are their own toggle) also needs Absurd Moves switched on.

## Double jump

Press Jump a second time in the air to jump again: a classic double jump, once per
jump - the game's own jump started again from your current height, its upward speed scaled by
`Second Jump Height Multiplier` (1.5 by default, so it goes well above a jump off the ground).
Fall damage counts from where the double jump started.

### `[Double Jump]`

| Setting | Default | What it does |
|---|---|---|
| `Double Jump Enabled` | true | Turns the double jump on or off. |
| `Second Jump Height Multiplier` | 1.5 | Scales the second jump's upward speed against a ground jump (1 = the same; height grows with the square). |

## Ground pound

Press Crouch in the air and you slam straight down at twice your fall speed, taking no fall damage.
You land in a concussion wave that shoves every enemy around you back, staggers and hurts them (more
up close), and throws up a ring of debris. It only starts if you're at least `Min Height` above the
floor, so a crouch-jump never sets it off. The landing is broadcast: everyone sees the debris, and
the host shoves the enemies, so it works for the whole team. Needs the GTFO-API plugin.

### `[Ground Pound]`

| Setting | Default | What it does |
|---|---|---|
| `Ground Pound Enabled` | true | Turns the ground pound on or off. |
| `Min Height (meters)` | 2 | How high above the floor you must be to start one. |
| `Min Slam Speed (m/s)` | 12 | Slowest the slam drops; it doubles your fall speed when that's faster. |
| `Shockwave Radius (meters)` | 5 | How far the shockwave reaches. |
| `Pushback Distance (meters)` | 4 | How far an enemy right next to you is shoved (less further out). |
| `Shockwave Damage` | 10 | Damage to each enemy caught in it (less further out). 0 = push only. |
| `Shockwave Stagger` | 3 | Stagger multiplier of that hit. |
| `Debris Count` | 10 | Dust-and-scorch bursts scattered around the landing (at most 50). |
| `Impact Sound` | `STINGER_LOW_BOOM` | Played on landing. Empty = silent. |

## Mantling

Jump at a ledge while holding forward. If its top is no higher than your eyes and there's room to
stand up there, you're pulled up the face and over onto it. Your gun drops out of the way for the
pull-up, like a vault. Anything below knee height is left to the game's own step-up.

### `[Mantling]`

| Setting | Default | What it does |
|---|---|---|
| `Mantling Enabled` | true | Turns mantling on or off. |
| `Mantle Duration (seconds)` | 0.35 | How long the pull-up takes. |
| `Ledge Reach (meters)` | 0.8 | How close the ledge's face must be in front of you. |
| `Eye Level Margin (meters)` | 0.1 | How far above eye level a ledge top can still be grabbed. |

## Slide

Crouch while sprinting to slide on in the direction you were running. The slide holds most of your
speed at first, then brakes into a stop, covering about 14 m at the defaults. Movement keys are
ignored while you slide and the camera stays free. You can slide past an open doorway, turn and shoot
into it, and keep sliding the same way. Standing up, jumping or hitting a wall ends it.

### `[Slide]`

| Setting | Default | What it does |
|---|---|---|
| `Slide Enabled` | true | Turns the slide on or off. |
| `Slide Start Speed (m/s)` | 9 | Speed the slide starts at (more if you were already faster). |
| `Slide Duration (seconds)` | 2.4 | How long a slide lasts; it covers about two thirds of start speed x duration. |

## Dodge

Hold Shift and double-tap W, A, S or D to dodge that way, relative to where you're looking. It's
mainly for ducking out of a homing projectile's path on short notice. Walls stop it short.

### `[Dodge]`

| Setting | Default | What it does |
|---|---|---|
| `Dodge Enabled` | true | Turns the dodge on or off. |
| `Dodge Distance (meters)` | 6 | How far a full dodge goes. |
| `Dodge Duration (seconds)` | 0.25 | How long a full dodge takes; distance / duration is its speed. |
| `Dodge Cooldown (seconds)` | 1.5 | Wait before another dodge can start. |
| `Dodge Double-Tap Window (seconds)` | 0.5 | How quickly the second tap must follow the first. |

## Bullet time

Tap Left Alt and the whole game slows down, Max Payne style - you, your teammates, bots, enemies and
projectiles alike. It eases into slow motion over 0.4 s, holds, then eases back up to full speed over 0.8 s - 5 seconds in all.
Once its cooldown is over, the event log on the left says
"Bullet time ready". The sound
drops to a hollow, muffled mix, with a deep boom going in and a heartbeat underneath. It's broadcast,
so the host slows the enemies for the whole team. Needs the GTFO-API plugin.

### `[Bullet Time]`

| Setting | Default | What it does |
|---|---|---|
| `Enabled` | true | Turns bullet time on or off. |
| `Key` | LeftAlt | Starts bullet time as you let go of it. A press with another key in it (Alt+Tab, Alt+Enter) or one that loses the game focus doesn't count, and presses while it's running or cooling down do nothing. |
| `DurationSeconds` | 5 | How long it lasts in all, in real seconds, counting the 0.4 s ease in and 0.8 s ease out (so the default holds at full slow motion for 3.8 s). Never shorter than 1.2 s. |
| `CooldownSeconds` | 10 | Wait after it ends before it can start again. |
| `TimeScale` | 0.3 | How fast the game runs during bullet time (1 = normal). |
| `SoundMuffle` | `SOUND_EFFECT_MUFFLE=100, GAME_MENU_MUFFLE=100` | Wwise mix parameters pushed for the hollow sound, restored after. |
| `EnterSound` / `ExitSound` | `STINGER_LOW_BOOM` / `COMBAT_FADING_PULSE_LEVEL_1` | Played going in and coming out. |
| `LoopSound` / `LoopStopSound` | `ENEMYHEARTBEATLOOP` / `ENEMYHEARTBEATLOOPEND` | The heartbeat under it. |

Any [Unity `KeyCode`](https://docs.unity3d.com/ScriptReference/KeyCode.html) name works for `Key`.

## Fire on the move

Vanilla GTFO ignores the trigger while you're sprinting or in the air. With these on, your guns fire
while you jump, fall or sprint, and sprinting carries on while you shoot. Only your
own guns are affected, not teammates' or bots'.

Shooting mid-sprint brings your gun up out of its lowered sprint pose for as long as you hold the
trigger, and for a moment after. The shots, tracers and muzzle flash then leave a barrel pointed where
you're aiming. Teammates already see your gun tracking where you look while you sprint. In the air the
gun is never lowered in the first place.

### `[Fire On The Move]`

| Setting | Default | What it does |
|---|---|---|
| `Fire Mid-Air Enabled` | true | Fire while jumping or falling. |
| `Fire While Running Enabled` | true | Fire while sprinting. |

## Infinite ammo and health

**Insert** toggles infinite health (no damage to anyone on the team, bots included, whoever presses it) and
**Delete** infinite ammo (every gun, tool, placed sentry and grenade stays full for the whole team, no reloads), each announced in the event
log and always in the console too. Both start off. They're the same two keys GTFO.EZ and its Absurd
companions all have, from one shared copy of the code written in GTFO.EZ; with several of the mods
installed, only one answers, so a press never toggles twice. (GTFO.CrashTestDummies dropped
Insert/Delete, and this shared code, in its own v17.)

## Compatibility

- **GTFO.EZ** used to have the dodge (as its dash), and **GTFO.EZ.AbsurdWeapons** used to have bullet
  time. Both moved here. Running an older build of either alongside this mod gives you two of the
  same move: switch one of them off in its config.
- **Faster sprint and more stamina** used to live here (`[Run Speed]`/`[Stamina]`); both moved to
  **GTFO.EZ** - its own `[Movement] Run Speed Multiplier` already did the sprint side (this mod's
  copy only stacked another multiplier on top of it), and its new `[Stamina] Stamina Multiplier`
  now covers the stamina side too, so you get both with or without this mod installed.
- Double jump, ground pound, mantling, slide and fire on the move are built on the game's own
  movement and weapon code and confirmed in play. If one misbehaves, switch it off and the rest
  keep working.

## Versioning

v1, v2, v3... - major-only bumps, matching the convention across other MindAttic projects.
Thunderstore requires `manifest.json`'s `version_number` to be strict `x.y.z`, so "v17" is `17.0.0`
(minor/patch stay 0). `package/CHANGELOG.md` headers use the short `vN` form.

The GTFO.EZ family - GTFO.EZ, GTFO.EZ.AbsurdWeapons, GTFO.EZ.AbsurdMoves and GTFO.EZ.AbsurdItems -
shares one version number, so a matching set is easy to spot: the companions' first release is
**v13**, in step with GTFO.EZ (not v1), and every release of the family bumps all four together
(current: v20).

## Building

```powershell
.\build.ps1                         # builds into the "GTFO - Easy Mode DEV" mod-manager profile and zips package\ into dist\
.\build.ps1 -Profile "My Profile"   # another Thunderstore Mod Manager profile
```

Needs the .NET 6 SDK and a GTFO profile with BepInEx and GTFO-API installed (BepInEx's `core` and
`interop` DLLs and `plugins\GTFO-API.dll` are the build references).

## Project layout

```
src/GTFO.EZ.AbsurdMoves/
  Plugin.cs            BepInEx entry point: binds each move's config, applies Harmony patches
  Moves.cs             Per-frame driver for the local player's movement, and shared helpers
  MovesToggle.cs       Home: switches every move on/off, off at the start of every level
  AirMoves.cs          Double jump
  ConfigMigrations.cs  One-time moves of a changed default onto configs still on the old one
  GroundPound.cs       Ground pound and its shockwave (host-side enemy pushes)
  Mantle.cs            Pull-up onto ledges no higher than eye level
  Slide.cs             Crouch-while-sprinting slide
  Dodge.cs             Shift + double-tap W/A/S/D dodge
  BulletTime.cs        Left Alt: slows the whole game clock (Time.timeScale, eased in and out) with a muffled Wwise mix
  FireMidAir.cs        Lets your guns fire in the air
  FireWhileRunning.cs  Lets your guns fire while sprinting
  Sounds.cs            One-shot and looping Wwise sound helpers
  Shared/              linked from external/GTFO.EZ.Shared (a git submodule; change it there):
                       SharedOwner, the Insert/Delete toggles, MessageHelper and RemoteEvents
                       (throw-safe GTFO-API network events, used by bullet time and ground pound)
package/               Thunderstore manifest, icon (256x256, from logo.png), README and changelog
logo.png               Full-size logo
```
