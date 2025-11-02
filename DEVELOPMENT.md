# Development Guide - The Contractor

## Project Overview

This SPTarkov mod adds a new trader called "The Contractor" who provides random daily and weekly quests to players.

## Project Structure

```
TheContractor/
├── the_contractor/              # C# Source Code
│   ├── ModMetadata.cs          # Mod metadata and version info
│   ├── AddTraderWithAssortJson.cs  # Main trader initialization
│   ├── AddCustomTraderHelper.cs    # Helper methods for trader setup
│   ├── QuestGenerator.cs       # Quest generation logic (TODO)
│   ├── QuestTemplates.cs       # Quest templates and types
│   ├── QuestRewards.cs         # Reward calculation system
│   └── Properties/
│       └── AssemblyInfo.cs     # Assembly information
├── data/                        # Trader Data Files
│   ├── base.json               # Trader configuration
│   ├── assort.json             # Trader inventory (empty for quest-giver)
│   ├── dialogue.json           # Trader dialogue lines
│   └── TheContractor.png       # Trader portrait (needs to be added)
├── the_contractor.csproj        # C# Project file
├── the_contractor.sln           # Visual Studio solution
└── README.md                    # User documentation
```

## Current Implementation Status

### ✅ Completed
- [x] Basic trader boilerplate
- [x] Trader registration and initialization
- [x] Trader configuration (4 loyalty levels)
- [x] Quest template system foundation
- [x] Quest reward calculation framework
- [x] Project structure and build configuration

### 🚧 In Progress / TODO
- [ ] Implement actual quest generation logic
- [ ] Add quest persistence (save/load quest state)
- [ ] Implement daily quest rotation (24-hour timer)
- [ ] Implement weekly quest rotation (7-day timer)
- [ ] Create specific quest templates for each type:
  - [ ] Elimination quests
  - [ ] Collection quests
  - [ ] Survival quests
  - [ ] Extraction quests
  - [ ] Mark location quests
  - [ ] Place item quests
  - [ ] Find in raid quests
- [ ] Add item rewards to quest completion
- [ ] Create trader portrait image
- [ ] Add localization support for multiple languages
- [ ] Add configuration file for customization

## Building the Mod

### Prerequisites
- Visual Studio 2019 or later
- .NET 9.0 SDK
- SPTarkov Server (for testing)

### Build Steps

1. **Open the solution**
   ```
   Open the_contractor.sln in Visual Studio
   ```

2. **Restore dependencies**
   - Right-click solution → Restore NuGet Packages
   - Ensure SPTarkov.Server.Core references are resolved

3. **Build the project**
   - Set configuration to **Release**
   - Build → Build Solution (Ctrl+Shift+B)
   - Output DLL will be in `bin/Release/the_contractor.dll`

4. **Deploy to SPT**
   - Copy the entire mod folder to `SPT/user/mods/`
   - Ensure the structure is:
     ```
     SPT/user/mods/TheContractor/
     ├── the_contractor.dll
     └── data/
         ├── base.json
         ├── assort.json
         ├── dialogue.json
         └── TheContractor.png
     ```

## Development Notes

### Lint Errors
The C# files contain some decompiled code patterns (like `<field>P` backing fields) that may show lint errors in the IDE. These are intentional to match the SPTarkov modding pattern and will compile correctly.

### Quest System Architecture

The quest system is designed with three main components:

1. **QuestGenerator** - Handles the generation and refresh logic
2. **QuestTemplates** - Defines quest types, difficulties, and templates
3. **QuestRewards** - Calculates and distributes rewards

### Next Steps for Implementation

1. **Quest Generation Logic**
   - Implement `GenerateDailyQuest()` in QuestGenerator.cs
   - Implement `GenerateWeeklyQuest()` in QuestGenerator.cs
   - Add quest data structures to match SPT's quest format

2. **Time-Based Refresh**
   - Implement timer system for daily/weekly resets
   - Store last refresh timestamps
   - Check timestamps on server start and player login

3. **Quest Persistence**
   - Save active quests to player profile
   - Load quests on player login
   - Track quest progress

4. **Integration with SPT Quest System**
   - Hook into SPT's quest completion events
   - Register quests with the database
   - Handle quest acceptance and completion

## Testing

1. Start SPT server with the mod installed
2. Check server console for: `[THE CONTRACTOR has arrived in Tarkov]`
3. Launch game and check traders menu
4. Verify "The Contractor" appears with correct loyalty levels

## Customization

Edit `data/base.json` to customize:
- Trader ID
- Currency type (USD, EUR, RUB)
- Loyalty level requirements
- Grid height for inventory

## Resources

- [SPTarkov Modding Documentation](https://dev.sp-tarkov.com/)
- [SPTarkov Discord](https://discord.gg/spt-tarkov)
- Example mod reference: `ExampleMod/the_german_trader`

## License

MIT License - See README.md for details
