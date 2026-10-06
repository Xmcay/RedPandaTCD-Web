Red Panda TCD

Red Panda TCD is a browser-based tactical card game built with Blazor WebAssembly and C#. Players construct decks backed by different data structures, deploy Characters and Attacks, manage Energy, and compete across several game modes.

The project combines card-game strategy with interactive demonstrations of common data structures.

Features


Tactical Card Duel

- Turn-based matches with four phases:
  - Utility
  - Placement
  - Attack
  - End
- Initiative determines which player acts first.
- Energy is used to deploy cards and activate abilities.
- Characters have Entry, Passive, and Active abilities.
- Attacks can include effects such as:
  - Piercing
  - Multi-Hit
  - Fatigue
  - Resting
- Battle Log records actions and combat resolution.
- Victory, defeat, draw, surrender, and board-observation states.

Data Structure Decks

Each deck uses a real data structure that affects card storage, retrieval, recycling, and available actions.

- Queue
  - First-In, First-Out retrieval
  - Peek at upcoming cards

- Priority Queue
  - Cards are retrieved according to priority
  - Characters and other card types use different priority rules

- Stack
  - Last-In, First-Out retrieval
  - Automatic Placement behavior

- Random List
  - Cards are selected from a randomized draw pool
  - Players can inspect the available pool

- Linked List
  - Cards are stored as linked nodes
  - Players can reorder upcoming nodes

Deck Builder

- Create custom decks containing 10 to 15 cards.
- Choose one of five data structures.
- Search cards by name or ID.
- Filter by card type and archetype.
- Arrange cards for order-dependent structures.
- Assign priorities for Priority Queue decks.
- Validate deck size and card-copy limits.
- Save decks for use in other game modes.

Game Modes

- Training
  - Play against a Bot using a selected deck.

- Pass and Play
  - Two players share one device.
  - Player hands remain hidden while the device is passed.

- Grand Tournament
  - Face three opponents in succession.
  - One deck is locked for the entire run.
  - One defeat ends the run.
  - Completing the Tournament awards the Master Strategist title.

- Battle Simulator
  - Run automated Bot-versus-Bot matches.
  - Run batches of 1, 10, 100, or 1,000 matches.
  - Compare wins, draws, average turns, and match lengths.
  - Inspect stalled matches, exceptions, and diagnostics.

Tutorials

Red Panda TCD contains nine tutorials:

1. Learn to Play
2. Queue
3. Priority Queue
4. Stack
5. Random List
6. Linked List
7. Deck Builder
8. Grand Tournament
9. Battle Simulator

The guided tutorials highlight important controls, restrict incorrect actions where necessary, and track completion progress during the current session.

Technology

- C#
- .NET 10
- Blazor WebAssembly
- Razor Components
- HTML
- CSS
- GitHub Pages deployment support

Project Structure

RedPandaTCD-Web/
├── Component/       Reusable Razor components
├── Game/            Game engine, state, managers, and data structures
├── Layout/          Application and battle layouts
├── Pages/           Main application pages
├── Properties/      Local launch configuration
├── wwwroot/         Static web assets
├── App.razor        Root application component
├── Program.cs       Application startup
└── RedPandaTCD-Web.csproj

Running the Project Locally
Requirements

Install the .NET 10 SDK.

Check the installed version:

dotnet --version

Build

From the project root:

dotnet clean
dotnet build

Run
dotnet watch


Open the local address displayed in the terminal.

Publishing

The project is designed as a standalone Blazor WebAssembly application and can be published as static website files.

Create a release build with:

dotnet publish --configuration Release


The generated static website is placed under the publish output's wwwroot directory.

Controls

Most interactions use the mouse or touchscreen:

Select cards and field slots by clicking them.
Use Continue to complete the current phase.
Open the deck during Utility to use its structure-specific action.
Use the Battle Log to inspect actions and effects.
Use Surrender to end an unfinished match.
Educational Purpose

Red Panda TCD demonstrates how abstract data structures can influence visible gameplay behavior.

Instead of presenting Queue, Stack, Priority Queue, Random List, and Linked List only as isolated programming exercises, the game connects each structure to card retrieval, deck inspection, placement, and recycling mechanics.

Current Status

The game currently includes:

Complete battle flow
Custom deck construction
Five playable deck structures
Bot opponents
Pass and Play
Grand Tournament
Battle Simulator
Nine guided tutorials
Responsive browser interface
License

No license has been selected yet.

Until a license is added, the source code remains under the repository owner's default copyright protections.
Author

Developed by Earth Rhoy O. Sanchez as a student software project.



