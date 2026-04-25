# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

"Hack the Planet!" — a 2-player turn-based strategy game built for Ludum Dare 59 (theme: Signal). Built with MonoGame 3.8 on .NET 8.0 (Windows).

## Build & Run

```bash
# Build
dotnet build ldjam59/HackThePlanet.csproj

# Run
dotnet run --project ldjam59/HackThePlanet.csproj

# The content pipeline runs automatically via MonoGame.Content.Builder.Task on build
```

There are no automated tests in this project.

## Architecture

### Entry Point & Game Loop

`ldjam59/Program.cs` → `HackThePlanetGame` (extends MonoGame `Game`). The game class is a singleton (`HackThePlanetGame.Instance`) that holds all loaded textures/fonts and the `GameState`. It owns one `FSMComponent` which drives the entire game via a finite state machine.

### FSM (Finite State Machine)

The FSM is the backbone of all game flow. Key files:

- `FSM/IState.cs` — interface with `Enter`, `Exit`, `Tick`
- `FSM/StateManager.cs` — holds the current state, calls `Enter`/`Exit` on transitions
- `FSM/BaseState<T>` — abstract base; implements the singleton pattern (`BaseState<T>.Instance`) so states are reused across transitions; manages adding/removing MonoGame `GameComponent`s for the duration of a state
- `Components/FSMComponent.cs` — wraps `StateManager` as a MonoGame `GameComponent`, calls `Tick` each update

States are singletons — they persist across visits and are not re-instantiated.

#### State Hierarchy

```
FSM/Gameplay/Flow/          # Top-level non-gameplay screens
    TitleScreenState
    InstructionsState
    GameOverState

FSM/Gameplay/               # In-game turn phases (all extend MainLoopGameState<T>)
    InitializeGameState     # Sets up board/players, transitions immediately
    HandoffScreenState      # "Pass to player N" screen between turns
    SummonState             # Choose unit type to deploy
    DeployUnitState         # Place chosen unit on board
    MoveUnitsState          # Select and move units
    UnitAttackState         # Select unit and attack adjacent enemies
    RunProgramState         # Choose: Kill Process or Process Scan
    KillProcessState        # AI ability: kill any enemy unit at cost of 2 cycles
    EndPlayerTurnState      # Check win condition, advance to next player
```

`MainLoopGameState<T>` is a mid-level base class (between `BaseState<T>` and the concrete gameplay states) that renders the board, units, agents, and the hackerman side panel on every in-game state.

### Components

All components extend either:
- `HtpComponent` (`GameComponent`) — update-only
- `HtpDrawableComponent` (`DrawableGameComponent`) — update + draw

Components implementing `IParentComponent` own child components; `BaseState` calls `AddComponents`/`RemoveComponents` on them automatically.

Notable components:
- `BackgroundGridComponent` — draws the 10x10 grid
- `UnitRenderComponent` — renders a single unit/agent on the grid; has `HasBeenUsed` and `IsGhost` flags
- `GameStateComponent` — HUD showing current player, cycles remaining
- `CursorComponent` — the selection/movement cursor
- `DeploymentMenuComponent` — unit selection menu during summon phase
- `ButtonComponent` — reusable UI button

### Models

- `GameState` — singleton (`GameState.Instance`); the single source of truth for game data; delegates board operations to `Board`
- `Board` — 10x10 flat array (`IUnit[100]`); tile index = `y * 10 + x`
- `Player` — holds a player name and their `Agent`
- `Agent : IUnit` — the AI piece; has `IsAlive`
- `Unit : IUnit` — drones/crawlers; has `Agent` owner, `UnitType`, `IsGhost`
- `IUnit` — interface: `TileIndex`, `HasActed`, `IsGhost`, `Type`
- `UnitType` — enum: `None`, `Drone`, `Crawler`

Cycles (mana): default 4 per turn. Costs: Ghost/Crawler = 1 cycle, Drone = 2 cycles, Kill Process = 2 cycles.

### Commands

Command pattern with undo support. `CommandStack` is a singleton. All game-mutating actions should go through `CommandStack.Instance.Execute(command)`.

Gameplay commands live in `Commands/Gameplay/`:
- `AddUnitToBoardCommand`
- `MoveUnitCommand`
- `KillUnitCommand`
- `RemoveUnitFromBoardCommand`

### Input

`Input/MouseClickState.cs` — tracks mouse click state for use by gameplay states.

### Rendering

`HackThePlanetGame.Draw` uses `SpriteSortMode.BackToFront` with `Layer` enum values (defined in `Components/Layer.cs`) to control draw order.

Resolution is fixed at 960x540. The grid occupies the left ~540px; the right panel shows the hackerman side image and HUD.
