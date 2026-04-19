# Hack The Planet!

A game for [LD59](https://ldjam.com).

## Developer Diary

[Read here](DIARY.md)

## Instructions 

- Turn based
- Requires two player

Turn-based game where you take controls of an AI who must clear the grid of their opposition to get their signal through. You are aided by:

- A drone - Can move 1 square at a time, costs 2 cycle to spin up
- A crawler - Can move 1 square at a time, costs 1 cycle to spin up
- A drone ghost - Can move 1 square, cannot attack, costs 1 cycle to spin up
- A crawler ghost - Can move 1 square, cannot attack, costs 1 cycle to spin up
- An AI that can kill a process on the grid from anywhere (2 cycles), or perform a process scan to identify any ghosts in a radius of 4 from the AI. The AI cannot attack other AI or units like the crawler or drone

An AI cannot be harmed by another AI (rules of robotics or something), but an AI can create a drone or a crawler that CAN harm another AI. So you might want to create some of them!

## Turn Order

Each player gets a series of phases they can perform during their turn:
- Spawn new drones or crawlers
- Move drones, crawlers or the AI itself
- Attack nearby enemies
- Run a program; kill process or process scan

### Phases

#### Spawn New Drones

- New units can only be spawned in a square adjacent to the AI

#### Move Drones, Crawlers and AI

- Each unit and AI Agent can move once per turn in the Move phase

#### Attack Phase

- Any unit that is near an enemy unit can attack it. All units are one hit damage

#### Run a Program Phase

- The AI can, if you left enough cycles can kill a single enemy process or perform a process scan. Both of these actions cost 2 cycles so use them wisely!

## End Game

The goal is to defeat the enemy AI, get a unit (a drone or a crawler) close enough to the enemy AI and zzap!