# Generation procedural

This repository contains code and resources for procedural generation techniques. This repository contains Noise génération, Cellular Automata, SimpleRoomPlacement to create a room that uses the BSP to make a corridor connection between the rooms.

## Table of Contents

<details>
<summary>Table of Contents</summary>

- [Features](#features)
- [Getting Started](#getting-started)
- [BSP](#bsp)
- [Cellular Automata](#cellular-automata)
- [Noise Generation](#noise-generation)

</details>

## Features
- 2D Sampling
- Room Generation
- Noise generation
- Using [FastNoiseLite library](https://github.com/Auburn/FastNoiseLite/tree/master)
- Using [Unitask](https://github.com/Cysharp/UniTask)
- Using professor generation base

## Getting Started
### Performance Comparisons
| Processor             | Graphic Card    | RAM               |
|-----------------------|-----------------|-------------------|
| Intel Core I9 13980HX | 4050 RTX Laptop | 16Go 4800MGz DDR5 |

To create a script, you need to create a new scriptable object, you will use this logic
```csharp
protected override async UniTask ApplyGeneration(CancellationToken cancellationToken)
{
//Define variables

    // Check for cancellation
    cancellationToken.ThrowIfCancellationRequested();

    // Code logique

    // Waiting between steps to see the result.
    await UniTask.Delay(GridGenerator.StepDelay, cancellationToken: cancellationToken);
}
```

Once it's done, you can generate the "SimpleRoomPlacement" assets.
<img src="Documentation/AssetPreview.png?raw=true"/>

### First Generate a room 
To generate a room, we need to create a room and place it on the grid.
```csharp
private void PlaceRoom(RectInt room)
{
    for (int ix = room.xMin; ix < room.xMax; ix++)
    {
        for (int iy = room.yMin; iy < room.yMax; iy++)
        {
            if (!Grid.TryGetCellByCoordinates(ix, iy, out var cell)) 
                continue;
            AddTileToCell(cell, ROOM_TILE_NAME, true);
        }
    }
}
```

### BSP

This script will generate bunch of rooms and place corridor between them. To create a dungeon
<img src="Documentation/BSP.png?raw=true"/>

### Cellular Automata

This script generate terrain, using basic noises with a verification if there is 4 grass around, it create grass, and if not, it create water. With a multi layer check, in back ground, it permit to not create only water or only grass.
<img src="Documentation/CellularAutomata.png?raw=true"/>

### Noise Generation
This script generate a noise map, and use it to create a terrain using the library FastNoiseLite.
<img src="Documentation/NoiseGenerator.png?raw=true"/>