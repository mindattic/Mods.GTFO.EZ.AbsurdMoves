---
codex: 1
project: GTFO.EZ.AbsurdMoves
code: GAM
layer: stories
status: living
updated: 2026-10-18
---
<!-- GENERATED from MindAttic.Tickets/data/Tickets.db by `tickets db render`. Do not edit: this file is rewritten from the database; change stories and tickets with the tickets CLI. -->

# GTFO.EZ.AbsurdMoves — User Stories
> ✅ done (shipped & tested) · 🟡 partial · ⬜ planned. Every ✅ cites the test.

## Priority backlog
_Nothing recorded yet._

## Tickets
> Permanent log of every change and fix — this repo's ticket system. Rows are append-only: never
> delete or reword one; only move its Status (open → closed / wont-do) and fill in Ref. Status is
> a word, not a glyph, so tickets don't count toward the story index. Keep this section last.

| ID | Date | Type | Status | Summary | Ref |
|---|---|---|---|---|---|
| GAM-T1 | 2026-10-18 | docs | closed | Start the permanent ticket log in the workflow database | — |
| GAM-T2 | 2026-10-18 | feature | closed | Dodge (Shift + double-tap W/A/S/D) moved here from GTFO.EZ, with a Dodge Enabled on/off toggle in the .cfg | Release build clean (0 warnings) |
| GAM-T3 | 2026-10-18 | feature | closed | Bullet Time (tap Left Alt: enemies slow to a crawl, muffled sound) moved here from GTFO.AbsurdWeapons, keeping its Enabled toggle in the .cfg | Release build clean (0 warnings) |
| GAM-T4 | 2026-10-18 | feature | closed | Wall running and double jump: second Space mid-air near a wall runs along it, otherwise double-jumps (hold Space to glide); continuous burn loop sound; WallRunning and DoubleJump toggles in the .cfg | Release build clean (0 warnings); not yet confirmed in play |
| GAM-T5 | 2026-10-18 | feature | closed | Fire while jumping or mid-air, with an on/off toggle in the .cfg | Release build clean (0 warnings); not yet confirmed in play |
| GAM-T6 | 2026-10-18 | feature | closed | Fire while running (sprinting), with an on/off toggle in the .cfg | Release build clean (0 warnings); not yet confirmed in play |
| GAM-T7 | 2026-10-18 | feature | closed | Slide: crouch while sprinting to slide on in that direction while aiming freely, with an on/off toggle in the .cfg | Release build clean (0 warnings); not yet confirmed in play |
| GAM-T8 | 2026-10-18 | feature | closed | Run speed and stamina: on/off toggles plus Max Run Speed and Stamina multipliers in the .cfg | Release build clean (0 warnings); not yet confirmed in play |
| GAM-T9 | 2026-10-18 | feature | closed | Ground Pound: crouch mid-air (high enough above the ground) to slam down at double fall speed with no fall damage, landing in a shockwave that pushes enemies back with debris; on/off toggle in the .cfg | Release build clean (0 warnings); not yet confirmed in play |
| GAM-T10 | 2026-10-18 | docs | closed | Logo wired in as the package icon (also shown for local installs), full manifest description, README covering every move and its .cfg toggle | Release build clean (0 warnings); not yet confirmed in play |
| GAM-T11 | 2026-10-18 | fix | closed | Sprint-firing kept the first-person gun in its lowered sprint pose, so shots and tracers left a barrel pointed at the floor; it now comes up level while firing. Double jumps and wall jumps also play the gun's own jump sway | Release build clean; not yet confirmed in play |
| GAM-T12 | 2026-10-18 | fix | closed | Slide bled speed too fast and movement keys steered it, so turning to shoot pulled you off line; it now holds speed then brakes (about 7 m) and ignores movement keys | Release build clean; not yet confirmed in play |
| GAM-T13 | 2026-10-18 | feature | closed | Mantling: jumping at a ledge no higher than eye level while holding forward pulls you up onto it (room to stand checked, gun lowered for the pull-up); Mantling Enabled toggle | Release build clean; not yet confirmed in play |
| GAM-T14 | 2026-10-18 | fix | closed | Review fixes: moves reset on death/level change (stuck lowered gun, stale mantle/pound into next level); dodge once per frame with host bots; slide input zeroing skips bots and replaces the game's crouch-push; pound/mantle/dodge exclude each other; pound only lands on your feet, times out, restores game fall immunity, radius 0 safe; glide always sinks; teammate bullet-time trigger never refused; mantle uses live transform | Release build clean (0 warnings); not yet confirmed in play |
| GAM-T15 | 2026-10-18 | feature | closed | Shared Insert/Delete infinite ammo/health toggles imported from GTFO.EZ (Shared/, published by GTFO.EZ tools/publish-shared.ps1) - user request 2026-10-18 | d051093 |
| GAM-T16 | 2026-10-18 | fix | closed | Ground pound started inside the post-landing grace window recorded the mod's own fall-damage immunity as the game's, leaving fall damage off for good | — |
| GAM-T17 | 2026-10-18 | fix | closed | Slide start speed ignored how fast you were already going, though the README promises more if you were already faster | — |
| GAM-T18 | 2026-10-18 | fix | closed | Fire mid-air left the locomotion state faked as Stand if the gun's Update threw, since the restore was a postfix | — |
| GAM-T19 | 2026-10-18 | fix | closed | Dodge counted a first tap made without Shift held, so tap then Shift+tap dodged | — |
| GAM-T20 | 2026-10-18 | fix | closed | Ground pound config unvalidated: negative Pushback Distance pulled enemies in, huge Debris Count spawned unbounded effects | — |
| GAM-T21 | 2026-10-18 | fix | closed | Shared code from GTFO.EZ (G.EZ-T67/T68): one warden-message flag across mods that can't get stuck, owner skips mods without shared code | — |
| GAM-T22 | 2026-10-18 | chore | closed | Shared code republished from GTFO.EZ: SharedOwner.Order now lists GTFO.AbsurdItems | e92c94e |
| GAM-T23 | 2026-10-18 | feature | closed | Master switch: [0. Mod] Enabled as the cfg's first entry; off means the mod loads nothing (user request) | — |
| GAM-T24 | 2026-10-18 | feature | closed | Shared code republished from GTFO.EZ: SharedOwner treats a switched-off GTFO.* mod as not installed, IsActive/TryGetSetting helpers | 6c7fcb0 |
| GAM-T25 | 2026-10-18 | fix | closed | Fire while running puts the running flag back in a finalizer, so a throwing gun Update can't leave the sprint reading as not running | 675ead4 |
| GAM-T26 | 2026-10-18 | fix | closed | Ground pound and bullet time network handlers catch their own errors, so a failure never escapes into GTFO-API's packet hook on SNet_Replication.RecieveBytes | 6686df5 |
| GAM-T27 | 2026-10-18 | fix | closed | Bullet time starts when its key is released on its own, so Alt+Tab (and other Alt chords) no longer sets it off for the whole team | 5e5c5a9 |
| GAM-T28 | 2026-10-18 | fix | closed | Shared code from GTFO.EZ (G.EZ-T72/T75): warden message never cuts the game's own short; infinite-health check can't throw | — |
| GAM-T29 | 2026-10-18 | chore | closed | Renamed to GTFO.EZ.AbsurdMoves (project, namespace, plugin GUID, network events, package name, docs) as a GTFO.EZ companion mod built on its core | 2bc02fc |
| GAM-T30 | 2026-10-18 | fix | closed | Double jump floated: holding Jump after it glided you down. Now a classic double jump - glide and its settings removed | — |
| GAM-T31 | 2026-10-18 | fix | closed | Shared code from GTFO.EZ (G.EZ-T78/T79): WardenMessage became MessageHelper; Insert/Delete announce in the event log | — |
| GAM-T32 | 2026-10-18 | fix | closed | Double jump is the game's own jump re-entered from the current height (PLOC_State.Jump), not a fixed upward speed; Double Jump Speed setting removed | — |
| GAM-T33 | 2026-10-18 | feature | closed | High jump: stay crouched on the ground until you flash, then press Jump to launch ~10 m up (the game's jump with a raised upward speed), no fall damage on landing (user request) | — |
| GAM-T34 | 2026-10-18 | feature | closed | Bullet time slows enemy projectiles too (ProjectileTargeting speed, lifetime extended) while players and bots keep full speed, and the event log says 'Bullet time ready' when the cooldown ends (user request) | — |
| GAM-T35 | 2026-10-19 | feature | closed | Bullet time slows the whole game clock (Time.timeScale, eased in and out) so everyone and everything is affected, Max Payne style; replaces per-enemy and per-projectile slowing; EnemySpeed becomes TimeScale (user request) | — |
| GAM-T36 | 2026-10-19 | chore | closed | Align the GTFO.EZ family on one version, v13 (user request), documented in the READMEs and changelog; README review corrections | manifest + Plugin.VERSION 13.0.0 |
| GAM-T37 | 2026-10-19 | chore | wont-do | Convert tracked source files to LF line endings via .gitattributes, per HOUSE-LAW-13 (low priority) | — |
| GAM-T38 | 2026-10-19 | docs | closed | READMEs said bullet time holds for 5 s; DurationSeconds (5) includes the 0.4 s ease in and 0.8 s ease out (bughunt for v14; uncommitted) | 31ccdc7 |
| GAM-T39 | 2026-10-19 | chore | closed | Remove wall running (too few long flat walls in GTFO's levels to be useful; user request) - the double jump now works anywhere in the air | AirMoves.cs |
| GAM-T40 | 2026-10-19 | feature | closed | Slide goes twice as far for cool factor (user request): Slide Duration 1.2 -> 2.4 s (~14 m), with a one-time config migration for installs on the old default | Slide.cs, ConfigMigrations.cs |
| GAM-T41 | 2026-10-19 | feature | closed | Double jump's second jump needs to be higher: it re-enters the game's own jump at ground-jump height (AirMoves.cs) - add a Second Jump Height Multiplier setting under [Double Jump], default 1.5, scaling the upward speed after the re-entered Jump state (user request after play test) | 86ff163 (Release build clean, 0 warnings; not yet confirmed in play) |
| GAM-T42 | 2026-10-19 | fix | closed | High jump never launches: the game's own jump consumes the charged Jump press and leaves Crouch before HighJump.Tick runs (log: 'charged, but Jump left the crouch first (state Jump) - no launch'), so you get a normal-height jump - keep the charge across that frame and apply the Height velocity to the game's Jump state when a charged crouch turns into Jump | 12c17b0 |
| GAM-T43 | 2026-10-19 | chore | closed | Bump GTFO.EZ.AbsurdMoves to v14.0.0 with the GTFO.EZ family: manifest, changelog (wall running removed, longer slide, higher double jump, high jump fix) and READMEs (user request) | abdebaf |
| GAM-T44 | 2026-10-19 | docs | closed | Mark everything new as confirmed in play: drop the not-yet-confirmed notes from the READMEs and config text (user confirmed after in-game testing) | 208d397 |
| GAM-T45 | 2026-10-20 | fix | closed | A loop sound whose Post throws leaked a new CellSoundPlayer and logged a warning every frame it was touched (bullet time): the player is now recycled and the sound given up on (bughunt) | 1e49d20 |
| GAM-T46 | 2026-10-20 | fix | closed | A slide (Crouch state, 2.4 s) charged the high jump (1 s), so Jump late in a slide launched 10 m straight up - a slide no longer charges it (bughunt; user decision) | 53d629f |
| GAM-T47 | 2026-10-20 | docs | closed | Publish GTFO.EZ.AbsurdMoves docs to GitHub Pages via the shared Agency DocFX pipeline: run adopt-docs-site.ps1 (README.md as home page, docs/*.md as articles, API reference only where the code builds in CI), enable Pages with build_type=workflow, set the repo homepage URL, retire tools/build-readme.ps1 and README.htm, confirm the docs Actions run is green and https://mindattic.github.io/GTFO.EZ.AbsurdMoves/ returns 200. Depends on MAT-T8. | .github/workflows/docs.yml |
| GAM-T48 | 2026-10-20 | chore | open | Bug hunt: run an exhaustive /bughunt over the whole repo and fix every real bug found (each fix under its own fix ticket), then hunt again; close this only when a full pass finds nothing | user request 2026-10-20: approved repo sweep gap-fill (resets the untracked v14 bughunt partial) |
| GAM-T49 | 2026-10-21 | chore | closed | Add .prose; Update USER_STORIES.md | — |
| GAM-T50 | 2026-10-22 | docs | closed | Update USER_STORIES.md | — |
| GAM-T51 | 2026-10-24 | docs | closed | Update USER_STORIES.md | — |
| GAM-T52 | 2026-10-24 | feature | closed | End key toggles Absurd Moves on/off: off at run start, on lets the dodge/bullet time/pound/mantle/double jump/slide/high jump work, off again cancels whatever's in progress | MovesToggle.cs |
| GAM-T53 | 2026-10-24 | chore | closed | Remove RunAndStamina.cs (Run Speed/Stamina multipliers): Run Speed already duplicated GTFO.EZ's own setting, and Stamina moves there too as a new GTFO.EZ setting | folded into GAM-T52's commit (RunAndStamina.cs removed alongside the End toggle, since both touched the same Plugin.cs lines) |
| GAM-T54 | 2026-10-24 | chore | closed | Cut the v15 release: bump VERSION/manifest, move the End-toggle and Run Speed/Stamina-removal changelog entries from the already-shipped v14 into a new v15 section, rebuild and repackage dist/ | package/manifest.json |
| GAM-T55 | 2026-10-24 | docs | closed | Reread and refresh README/package README: fix every Insert/Delete wording for the swap (Insert is now health, Delete is now ammo) | README.md |
| GAM-T56 | 2026-10-24 | chore | closed | Cut the v16 release: Home replaces End as the Absurd Moves toggle, pull in GTFO.EZ.Shared's Insert/Delete swap (submodule bump), fix GTFO.* wording, rebuild and repackage dist/ | package/manifest.json |
| GAM-T57 | 2026-10-24 | chore | closed | Cut the v17 release: bump submodule to GTFO.EZ.Shared's Order cleanup, join CrashTestDummies/SkipDrop into the vN scheme, bump VERSION/manifest, rebuild and repackage dist/ | package/manifest.json |
