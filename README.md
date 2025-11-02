# The Contractor - SPTarkov Mod

A custom trader mod for SPTarkov that introduces "The Contractor" - a mysterious figure who provides random daily and weekly quests.

## Features

- **New Trader**: The Contractor with 4 loyalty levels
- **Daily Quests**: Random daily contracts (to be implemented)
- **Weekly Quests**: Random weekly contracts (to be implemented)
- **Quest Rewards**: Earn money, items, and reputation

## Installation

1. Download the latest release
2. Extract the mod folder to your SPT `user/mods/` directory
3. Start your SPT server
4. The Contractor will be available in-game

## Configuration

The trader can be configured by editing `data/base.json`:
- Trader ID: `contractor001`
- Currency: USD
- Loyalty levels: 4 levels with increasing requirements

## Development

This mod is built with C# targeting .NET 9.0 and SPTarkov Server Core.

### Project Structure
```
the_contractor/
├── the_contractor/          # C# source files
│   ├── ModMetadata.cs
│   ├── AddTraderWithAssortJson.cs
│   ├── AddCustomTraderHelper.cs
│   └── Properties/
│       └── AssemblyInfo.cs
├── data/                    # Trader data files
│   ├── base.json           # Trader configuration
│   ├── assort.json         # Trader inventory
│   └── dialogue.json       # Trader dialogue
└── the_contractor.sln      # Visual Studio solution
```

### Building

1. Open `the_contractor.sln` in Visual Studio
2. Build the solution (Release configuration)
3. The compiled DLL will be in `bin/Release/`

## TODO

- [ ] Implement quest generation system
- [ ] Add daily quest rotation
- [ ] Add weekly quest rotation
- [ ] Create quest templates
- [ ] Add quest rewards system
- [ ] Add trader portrait image

## Credits

Based on the SPTarkov custom trader example.

## License

MIT License
