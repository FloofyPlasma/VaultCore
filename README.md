# VaultCore Engine

VaultCore is an open-source game engine for building games. 
It is intended to be used as a library.
Game projects can reference VaultCore binares without including engine source code.

> [!NOTE]
> I plan on eventually publishing my engine to Nuget, this is currently not supported.

> [!CAUTION]
> The VaultCore engine is still in very early development and should not be used in production. There are breaking changes being made frequently.

## Features

- Placeholder 1
- Placeholder 2
- Placeholder 3
- FloofyPlasma was here..

## Demos

Placeholder! Sorry -FloofyPlasma

## Getting Started

### Requirements

- .NET SDK / C# compatible environment
- Access to Nuget
  - JetBrains Rider is suggested, Visual Studio works too.
  
### Building the Engine

1. Clone the repository:

```bash
git clone https://github.com/FloofyPlasma/VaultCore.git
cd Vaultcore
```

2. Build the project:

```bash
dotnet build -c Release
```

The compiled binaries will be located in `bin/Release`.

### Using the Engine in a Game

1. Add VaultCore as a Git submodule in your game repository:

```bash
git submodule add https://github.com/FloofyPlasma/VaultCore.git VaultCore
```

2. Reference the engine project or DLL in your game project.

### Contributing

I don't really have any main guidelines, just please try to maintain API compatibility to avoid breaking dependent games.

### License

VaultCore is licensed under the MIT License. See `LICENSE` for details.