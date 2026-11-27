---
layer: bible
status: living
---
# GTFO.EZ.AbsurdMoves — Project Bible

> Single source of truth for what GTFO.EZ.AbsurdMoves IS, is NOT, and the rules that keep it
> coherent. README says how to build and run it; this says how to think about it. The finish line
> lives in `docs/RELEASE.md`.

## 1. The one sentence {#GAM-§1}
A GTFO.EZ companion mod that adds player moves: double jump, ground pound, mantling, slide, dodge
and bullet time, plus firing mid-air and while sprinting.

## 2. The product promise {#GAM-§2}
- Every move has its own switch and its numbers can be retuned.
- Home turns all moves on or off at once (off at the start of every level); teammates are told.
- Bullet time uses the triggerer's duration and scale on every machine.
- In someone else's lobby the move settings are the host's (HostLevelSettings, G.EZ-T145).

## 3. What it is NOT {#GAM-§3}
- Not a difficulty, weapon or item mod (GTFO.EZ, AbsurdWeapons, AbsurdItems).
- No high jump: removed in v18 because it never launched reliably; double jump covers it.

## 4. Architecture canon {#GAM-§4}
```
 Plugin.cs ── ConfigMigrations
   ├─ MovesToggle (Home) ── Moves (shared state)
   ├─ AirMoves (double jump), GroundPound, Mantle, Slide, Dodge
   ├─ BulletTime (broadcast .v2 with duration and scale)
   ├─ FireMidAir, FireWhileRunning
   └─ HostLevelSettings (host's move settings)
 external/GTFO.EZ.Shared (SharedOwner, InfiniteToggles, MessageHelper, RemoteEvents)
```
### 4.1 Projects
`src/GTFO.EZ.AbsurdMoves` (plugin), `package/` (Thunderstore manifest and README).
### 4.2 Domain nouns
Move, Absurd Moves toggle, bullet time, pounder, host.
### 4.3 Key verbs
Drive moves through the game's own player controller so they sync as movement; broadcast toggle
and bullet-time events.

## 5. The Laws {#GAM-§5}
Inherits `MindAttic.HouseRules.md` by reference.
1. {#GAM-LAW-1} Moves go through the game's own controller and bindings (Jump, Crouch follow rebinds).
2. {#GAM-LAW-2} Turning moves off cancels whatever is in progress at once.
3. {#GAM-LAW-3} Shared code changes in GTFO.EZ.Shared, then the submodule moves.
4. {#GAM-LAW-4} Releases go out in lockstep with the GTFO.EZ family.

## 6. Verified state {#GAM-§6}
GtfoEZAbsurdMoves 21.0.0 is live on Thunderstore (API, 2026-11-27).

## 7. Active frontier {#GAM-§7}
See `docs/RELEASE.md`: the v22 family release.

## 8. Quality bar {#GAM-§8}
Builds clean with `build.ps1`; the move has a switch and works only while Absurd Moves is on;
`package/README.md` and the CHANGELOG describe it.

## 9. Glossary {#GAM-§9}
- **Absurd Moves toggle** — Home; the per-player master switch for every move.
- **Bullet time** — Left Alt slows the game for the team using the triggerer's settings.
