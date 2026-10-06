# Red Panda Tactical Card Duel

**Red Panda TCD (Tactical Card Duel)** is a browser-based tactical card game built with **C# and Blazor WebAssembly**.

The game combines tactical card-game mechanics with real data structures. Each deck is backed by a different data structure, and that structure directly affects how cards are drawn, inspected, placed, reordered, and recycled during a match.

The project was developed as a student software project with the goal of turning data-structure concepts into something interactive and playable rather than treating them only as programming exercises.

---

## Game Overview

Each player builds a deck of cards and attempts to reduce the opponent's **Energy to 0**.

Energy functions as both the player's health and an important resource for playing cards and using abilities.

Matches are divided into four phases:

1. **Utility**
2. **Placement**
3. **Attack**
4. **End**

Players alternate initiative between turns, while phases are resolved for both players before the match advances to the next phase.

Players must manage their Energy, characters, attacks, hand, deck structure, shields, abilities, and available actions to win.

---

## Core Gameplay

### Energy

- Players begin with **20 Energy**.
- Maximum Energy is **25**.
- Energy is used to perform actions and play cards.
- Energy also represents the player's remaining health.
- If a player's Energy reaches 0, the match ends.
- Energy can be recovered during the End Phase according to the current turn.

### Characters

Characters are the main units on the field.

A player can have one active Character at a time.

Characters can have:

- **Entry effects**
- **Passive abilities**
- **Active abilities**
- **HP**
- **Shield**
- Character-specific mechanics

When a Character is defeated, the player can deploy another Character according to the game's placement rules.

### Attacks

Players can have up to two Attacks prepared on the field.

Attacks can have different properties, including:

- Damage
- Piercing
- Multi-Hit
- Fatigue
- Resting
- Special effects

Attacks are placed during the Placement Phase and used during the Attack Phase.

Attack usage and fatigue are managed across turns, making attack timing an important part of the game.

### Utilities

Utilities provide additional actions and effects during the Utility Phase.

Examples include:

- Drawing and discarding cards
- Manipulating the deck
- Inspecting upcoming cards
- Reordering cards
- Recovering Energy
- Other utility-specific effects

The available actions can also depend on the player's selected deck structure.

### Shields

Characters can have Shields that protect them from incoming attacks.

Shield behavior is intentionally different from ordinary damage reduction. Depending on the attack, a Shield can act as a hit barrier, while effects such as Piercing can interact with it differently.

---

# Deck Data Structures

One of the defining features of Red Panda TCD is that every deck is backed by a real data structure.

The data structure is not just a label. It determines how cards are stored and retrieved and can provide different gameplay options.

## Queue

**First-In, First-Out (FIFO)**

Cards are drawn from the front of the deck in the order they were stored.

Queue decks emphasize planning around a known sequence of cards.

## Stack

**Last-In, First-Out (LIFO)**

The most recently available card is retrieved first.

Stack decks also use specialized placement behavior that automatically works through the available cards and field slots.

## Priority Queue

Cards are retrieved according to their assigned priority rather than simply following their position in the deck.

Priority Queue decks allow players to influence which cards become available first by assigning card priorities.

## Random List

Cards are drawn from a randomized collection.

Random List decks introduce more uncertainty into card retrieval and provide mechanics for inspecting the available draw pool.

Random List decks also use their own opening-hand behavior to ensure a Character can be available to begin the match.

## Linked List

Cards are stored as linked nodes.

Linked List decks allow players to manipulate the order of nodes, making card positioning part of the strategy.

The deck can be reordered through node manipulation rather than treating the deck as a simple fixed list.

---

# Deck Builder

The Deck Builder allows players to create and save custom decks.

Features include:

- Decks containing **10–15 cards**
- Selection of one of five deck structures
- Card search
- Card ID search
- Card-type filtering
- Archetype filtering
- Structure-dependent card ordering
- Priority assignment for Priority Queue decks
- Deck validation
- Card-copy restrictions
- Saved custom decks

The same deck definitions can then be used by the different game modes.

---

# Game Modes

## Training

Play a standard match against a Bot opponent.

The player selects a deck and plays through the normal game engine.

## Pass and Play

Two players can play on the same device.

The game handles the turn transition so players can pass the device while keeping the other player's hand hidden.

## Grand Tournament

The Grand Tournament is a multi-match challenge.

- Face three opponents in succession.
- Select one deck for the tournament run.
- The selected deck remains locked for the run.
- A defeat ends the tournament.
- Completing the tournament awards the **Master Strategist** title.

## Battle Simulator

The Battle Simulator is designed for automated testing and analysis of the game engine.

### Bot Fighter

Bot Fighter runs automated Bot-versus-Bot matches using the same game systems used by normal matches.

The simulator can run:

- A single match
- 10 matches
- 100 matches
- 1,000 matches

Results include information such as:

- Bot 1 wins
- Bot 2 wins
- Draws
- Completed matches
- Stalled matches
- Exceptions
- Average turn count
- Shortest match
- Longest match

The simulator uses the actual battle flow rather than maintaining a separate set of simulator-only gameplay rules.

### Battle Log Replay

Battle Log Replay is designed to work with a Battle Log copied directly from a completed match.

The workflow is:

1. Copy the Battle Log from a match.
2. Paste the log into the Simulator.
3. Select the deck used by Player 1.
4. Select the deck used by Player 2.
5. Load the match.
6. Analyze or replay the recorded match.

Replay information is stored separately from the human-readable Battle Log.

The visible Battle Log remains focused on readable match events, while additional structured information can be used by the Simulator.

This replay data can include:

- Turn information
- Player information
- Action types
- Card information
- Card types
- Deck structure
- Draw events
- Other information required to reconstruct match activity

The system is designed around structured game events rather than attempting to interpret gameplay by parsing English sentences from the displayed log.

---

# Battle Log

The Battle Log records what happens during a match.

It displays events by phase and provides a readable history of actions such as:

- Utilities
- Card placement
- Attacks
- Abilities
- Damage
- Character defeats
- Energy changes
- End-of-turn events

The log can be copied directly from the Battle page.

A copied Battle Log can then be supplied to the Simulator for replay and analysis.

---

# Tutorial System

Red Panda TCD includes guided tutorials for both gameplay and the data structures behind the decks.

Current tutorials include:

1. **Learn to Play**
2. **Queue**
3. **Priority Queue**
4. **Stack**
5. **Random List**
6. **Linked List**
7. **Deck Builder**
8. **Grand Tournament**
9. **Battle Simulator**

The tutorials explain the relevant mechanics while guiding the player through the interface.

Some tutorials can restrict or guide actions so that the player learns the intended mechanic without accidentally skipping the lesson.

---

# Technical Implementation

Red Panda TCD is built primarily with:

- **C#**
- **.NET 10**
- **Blazor WebAssembly**
- **Razor Components**
- **HTML**
- **CSS**

The game logic is separated from the UI where practical.

Major systems include:

- Battle engine
- Match state management
- Phase management
- Character management
- Attack management
- Utility management
- Placement management
- End-phase management
- Bot management
- Deck management
- Deck storage implementations
- Battle logging
- Tutorial state
- Tournament state
- Game session state

The game uses the same core systems for normal gameplay and automated simulation wherever possible.

---

# Data Structures and Algorithms

The project uses multiple data structures as actual gameplay systems.

The main implementations are:

- Queue
- Stack
- Priority Queue
- Random List
- Linked List

These are implemented through a common deck-storage interface while allowing each structure to maintain its own retrieval behavior.

This allows the game to demonstrate the difference between data structures through actual player interaction.

For example:

- A Queue changes the order in which cards become available.
- A Stack reverses the expected retrieval direction.
- A Priority Queue uses card priority to determine retrieval.
- A Random List introduces randomized retrieval.
- A Linked List allows node-level reordering.

The project therefore connects an abstract programming concept to a visible gameplay consequence.

---

# Project Structure

```text
RedPandaTCD-Web/
├── Components/          Reusable Razor components
├── Game/                Game engine, state, managers, cards, and data structures
├── Layout/              Application layouts and navigation
├── Pages/               Main application pages
├── Properties/          Local launch configuration
├── wwwroot/             Static web assets and CSS
├── App.razor            Root application component
├── Program.cs           Application startup and service registration
├── RedPandaTCD-Web.csproj
└── README.md
```

---

# Running the Project Locally

## Requirements

Install the **.NET 10 SDK**.

Check the installed version:

```bash
dotnet --version
```

## Build

From the project root:

```bash
dotnet clean
dotnet build
```

## Run

```bash
dotnet watch
```

Open the local address shown in the terminal.

---

# Publishing

The project is designed to run as a standalone Blazor WebAssembly application and can be published as static web content.

Create a Release build with:

```bash
dotnet publish --configuration Release
```

The generated website files are placed within the publish output.

---

# Controls

Most gameplay interactions use the mouse or touchscreen.

Common interactions include:

- Selecting cards
- Selecting field slots
- Using phase actions
- Continuing to the next phase
- Using structure-specific deck actions
- Inspecting the Battle Log
- Copying the Battle Log
- Surrendering an unfinished match

The exact available actions depend on the current phase, deck structure, cards, Energy, and game state.

---

# Educational Purpose

Red Panda TCD was designed to demonstrate how data structures can affect the behavior of a larger software system.

Instead of presenting Queue, Stack, Priority Queue, Random List, and Linked List as isolated examples, the project uses them as functional parts of a card game.

This creates a direct relationship between:

**Data Structure → Program Behavior → Game Mechanic → Player Experience**

The project also demonstrates software concepts including:

- Object-oriented programming
- State management
- Interfaces
- Collections
- Algorithms
- Game-state validation
- Turn and phase management
- Event logging
- Simulation
- Automated testing through repeated matches
- UI and game-logic separation

---

# Current Status

Red Panda TCD currently includes:

- Complete tactical battle system
- Character cards
- Attack cards
- Utility cards
- Energy system
- Shield mechanics
- Character abilities
- Multiple attack effects
- Four-phase turn system
- Initiative system
- Five playable deck structures
- Custom Deck Builder
- Bot opponents
- Pass and Play
- Grand Tournament
- Battle Log
- Guided tutorials
- Automated Battle Simulator
- Battle simulation statistics
- Structured Battle Log data for replay support

The Battle Simulator is currently being expanded to support both automated Bot Fighter matches and Battle Log-based replay and analysis.

---

# Development

The project is developed and maintained as an individual student software project.

Development focuses on keeping the core game rules centralized so that normal matches, Bot matches, and simulator matches can use the same underlying game systems rather than maintaining separate versions of the rules.

---

# Copyright

**Copyright © 2026 Earth Rhoy O. Sanchez. All rights reserved.**

Red Panda TCD, including its original game design, game rules, source code, written content, card designs, and other original project material, is the property of its author unless otherwise stated.

No license has been granted for redistribution, modification, or commercial use of the project.

Third-party libraries, frameworks, and other external materials remain subject to their respective licenses and copyrights.