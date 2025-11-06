# Generation procedural

This repository contains code and resources for procedural generation techniques. This repository contain Noise génération, Cellular Automata, SimpleRoomPlacement to create room who use the BSP to make corrider connection between the room.

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


