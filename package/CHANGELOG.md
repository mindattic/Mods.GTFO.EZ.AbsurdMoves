# Changelog

## Unreleased

- **Absurd Moves on/off is announced to the team.** Pressing Home still switches only your own moves,
  but every teammate's event log now says "Bob: Absurd Moves ON" (or OFF).
- **Bullet time is the same for everyone and says who started it.** The broadcast now carries the
  triggerer's duration and time scale, and every player's game uses those instead of its own config,
  so nobody's game speeds back up early or slows by a different amount. Everyone's event log says
  "Bullet time - triggered by Bob". (The network event was renamed, so an older install in the same
  lobby no longer syncs bullet time with this one.)
- **A v15 config's End toggle now migrates to Home.** v16 moved the Absurd Moves toggle to Home, but
  a config last saved by v15 kept `End`, which collides with GTFO.EZ.AbsurdWeapons' End toggle. It
  is rewritten to `Home` on load.
- **Turning Absurd Moves (or Fire While Running) off mid-sprint-fire restores the sprint gun pose.**
  The first-person weapon no longer stays in its aim pose for the rest of the sprint.
- **A looping sound that loses its position update is now stopped** instead of being forgotten and
  playing forever.
- The Thunderstore description names Home (not End) as the toggle key.

## v20

- Dependency bump: `BepInExPack_GTFO` 3.2.3 to 3.2.4.

## v19

- Moves in step with the GTFO.EZ family's version number (GTFO.EZ, GTFO.EZ.AbsurdWeapons,
  GTFO.EZ.AbsurdMoves, GTFO.EZ.AbsurdItems, GTFO.CrashTestDummies and GTFO.SkipDrop share one). No
  change in this mod's own behavior.

## v18

- **Removed: high jump.** It never reliably launched and the double jump already covers getting up
  high, so the crouch-charge, the flash light and `[High Jump]` are gone. An existing `.cfg`'s
  `[High Jump]` section stays as leftover lines that do nothing.

## v17

- `GTFO.EZ.Shared` submodule bump: `SharedOwner.Order` no longer lists `GTFO.CrashTestDummies`,
  which dropped Insert/Delete (and this submodule) in its own v17. No change in this mod's own
  behavior.
- `GTFO.CrashTestDummies` and `GTFO.SkipDrop` join the `vN` scheme this release, so every GTFO.*
  mod now shares one version number.

## v16

- **The Absurd Moves toggle moved from End to Home.** GTFO.EZ.AbsurdWeapons' own toggle moves the
  other way, onto End.
- **Insert and Delete swapped: Insert is now infinite health, Delete is now infinite ammo.** Both
  also always announce in the console now, not just the event log. Shared code
  (`GTFO.EZ.Shared`), so GTFO.EZ, its Absurd companions and GTFO.CrashTestDummies all pick it up.
- Moves in step with the GTFO.EZ family's version number (GTFO.EZ, GTFO.EZ.AbsurdWeapons,
  GTFO.EZ.AbsurdMoves and GTFO.EZ.AbsurdItems share one).

## v15

- **Absurd Moves is a toggle, off at the start of every level.** Press **End** to turn it on; every
  move below starts working. Press it again to turn it off: whatever's in progress is cancelled right
  away (an in-flight dodge stops, a running bullet time snaps back to normal speed, a pound/mantle/
  slide/charged high jump resets), and none of them start again until End is pressed again. The state
  is announced on-screen and in the console.
- **Faster sprint and more stamina moved to GTFO.EZ.** `[Run Speed]`/`[Stamina]` are gone from this
  mod - Run Speed only ever stacked a second multiplier on top of GTFO.EZ's own `[Movement] Run Speed
  Multiplier`, and Stamina's logic moved there as a new `[Stamina] Stamina Multiplier`, so both work
  with or without this mod installed.
- Moves in step with the GTFO.EZ family's version number (GTFO.EZ, GTFO.EZ.AbsurdWeapons,
  GTFO.EZ.AbsurdMoves and GTFO.EZ.AbsurdItems share one).

## v14

- **Removed: wall running.** GTFO's levels have too few long, flat walls for it to be useful. Its
  `[Wall Running]` and `[Air Move Sounds]` settings stay in an existing `.cfg` as leftover lines that
  do nothing; the double jump works anywhere in the air now, wall or no wall.
- **Double jump** goes higher: its upward speed is scaled by the new `[Double Jump] Second Jump
  Height Multiplier` (default 1.5), so the second jump clears well above a ground jump.
- **Bug fix:** the high jump never launched - the game's own jump took the charged press first, so
  you got a normal-height jump. The charge now carries into that jump. A slide no longer charges it,
  so Jump at the end of a long slide is a normal jump, not a 10 m launch.
- **Slide** goes twice as far: *Slide Duration* 1.2 s -> 2.4 s, about 14 m at the default speed. An
  install still on the old 1.2 is moved to 2.4 once; a value you set yourself is kept.
- Moves in step with the GTFO.EZ family's version number (GTFO.EZ, GTFO.EZ.AbsurdWeapons,
  GTFO.EZ.AbsurdMoves and GTFO.EZ.AbsurdItems share one).

## v13

- **Versioning:** the GTFO.EZ family - GTFO.EZ, GTFO.EZ.AbsurdWeapons, GTFO.EZ.AbsurdMoves and
  GTFO.EZ.AbsurdItems - now shares one version number, so this first release is **v13**, in step
  with GTFO.EZ, rather than v1.
- Renamed from GTFO.AbsurdMoves to **GTFO.EZ.AbsurdMoves** before release, as a GTFO.EZ companion mod built
  on GTFO.EZ's core (plugin GUID `MindAttic.GTFO.EZ.AbsurdMoves`, config file
  `MindAttic.GTFO.EZ.AbsurdMoves.cfg`, Thunderstore package `GtfoEZAbsurdMoves`).
- **Master switch**: `[0. Mod] Enabled` (default `true`) is the config file's first entry; set it to
  `false` and the whole mod is off - nothing is patched, no move or hotkey works.
- **Insert** (infinite ammo) and **Delete** (infinite health) toggles, announced as warden
  messages - the shared keys every GTFO.* mod has, so they never clash with several installed.
- Initial release. Every move has its own on/off switch in the config.
- **Wall running**: in the air next to a wall, press Jump again to run along it; Jump again kicks off.
- **Double jump**: press Jump again in the air for a whole new jump from where you are - the
  game's own jump, no glide or float.
  A small burning loop plays while wall running.
- **High jump**: stay crouched on the ground until you flash, then press Jump to launch about 10 m
  straight up, with no fall damage on the landing.
- **Ground pound**: press Crouch high enough in the air to slam down at double fall speed with no fall
  damage, landing in a shockwave that shoves and staggers enemies, with debris.
- **Mantling**: jump at a ledge no higher than your eyes while holding forward to pull up onto it.
- **Slide**: crouch while sprinting to slide on that way while aiming freely.
- **Fire on the move**: guns fire mid-air and while sprinting.
- **Faster sprint** (x1.25) and **more stamina** (x1.5), each a multiplier with its own switch.
- **Dodge**: hold Shift and double-tap W/A/S/D to dodge that way (moved here from GTFO.EZ's dash).
- **Bullet Time**: tap Left Alt to slow the whole game - everyone and everything - for 5 s, easing in and
  back out, Max Payne style (the event log says when it's ready again), with a muffled sound mix
  (moved here from GTFO.EZ.AbsurdWeapons). It starts as the key is let go, so Alt+Tab never sets it off.
