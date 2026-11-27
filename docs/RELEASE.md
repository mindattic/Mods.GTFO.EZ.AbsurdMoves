---
layer: release
status: living
---
# GTFO.EZ.AbsurdMoves — Release charter

> The one finish line this repo is working toward. Every open ticket in this repo sits under one of the epics
> below, and every epic is part of the milestone. Work that does not fit is out of scope until this milestone is
> reached (owner, 2026-11-27, MAT-T319).

## 1. What this is
A GTFO.EZ companion mod that adds player moves: double jump, ground pound, mantling, slide, dodge and bullet time.

## 2. The milestone: AbsurdMoves v22 release {#milestone}
- **Milestone:** MS-60, "Mods.GTFO.EZ.AbsurdMoves v22 release"
- **Published means:** GtfoEZAbsurdMoves v22 on Thunderstore
  (https://thunderstore.io/c/gtfo/p/MindAttic/GtfoEZAbsurdMoves/), shipped by the family `/publish`.
- **The natural stopping point:** move settings follow the host (G.EZ-T145, landed) and the README matches v22.
  The move set is complete.

## 3. Done when (acceptance checks)
1. build — `src/GTFO.EZ.AbsurdMoves/GTFO.EZ.AbsurdMoves.csproj` builds clean with `build.ps1` on main.
2. exists — the Thunderstore listing shows v22.
3. signoff — the owner plays a co-op drop: Home toggles moves and bullet time reaches every player.

## 4. Epics, in priority order
| Priority | Epic (ticket) | What it delivers | Features |
|---|---|---|---|
| 900 | GAM-T73 v22 release readiness | README ready for v22 | GAM-T74 README matches v22 (990): GAM-T75 |

The release itself is G.EZ-T143 in Mods.GTFO.EZ (the family `/publish`).

## 5. Out of scope (not before this milestone)
- New moves (high jump was removed on purpose in v18).
- Syncing per-player movement numbers.
- Open-ended bug hunts.

## 6. Current verified state
- GtfoEZAbsurdMoves 21.0.0 is live on Thunderstore (API, 2026-11-27).
- Host move settings (f1f5f6f G.EZ-T145); bullet-time broadcast fix (8c3a217 GAM-T72); team-visible toggle and
  bullet time (9b7ea6b GAM-T66).

## 7. Decision record
- 2026-11-27 — charter drafted from README/git log/tickets; Legion vote: Approve, plurality quorum reached,
  5 of 9 voters answered, consensus 1.0, no dissent.
- Triage (G.EZ-T147): 3 open → done 2 (T64, T65), duplicate 0, wont-do 1 (T71), kept 0, new tasks 1 (T75).
