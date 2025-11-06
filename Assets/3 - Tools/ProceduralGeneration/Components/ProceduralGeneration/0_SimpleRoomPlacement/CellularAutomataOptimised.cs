using System.Threading;
using Components.ProceduralGeneration;
using Cysharp.Threading.Tasks;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

/// <summary>
/// Optimized Cellular Automata generation with Burst for computation
/// and progressive chunk-by-chunk application to the grid.
/// </summary>
[CreateAssetMenu(menuName = "Procedural Generation Method/CellularAutomata (Chunked)")]
public class CellularAutomataOptimised : ProceduralGenerationMethod
{
    [Header("Automata Settings")]
    [Range(0,100)] [SerializeField] private int _noiseDensity = 50;
    [Min(1)] [SerializeField] private int _iterations = 5;
    [SerializeField] private bool _solidBorder = true;
    [Range(0,8)] [SerializeField] private int _birthThreshold = 5;
    [Range(0,8)] [SerializeField] private int _surviveThreshold = 4;
    [SerializeField] private uint _seed = 0;

    [Header("Application Settings")]
    [Tooltip("Size of chunks applied per frame. Example: 32 means 32x32 tiles per update.")]
    [Range(8,128)] [SerializeField] private int _applyChunkSize = 32;

    protected override async UniTask ApplyGeneration(CancellationToken cancellationToken)
    {
        int width = Grid.Width;
        int height = Grid.Lenght;
        int count = width * height;

        var cur = new NativeArray<byte>(count, Allocator.TempJob);
        var next = new NativeArray<byte>(count, Allocator.TempJob);

        uint seed = _seed != 0 ? _seed : (uint)UnityEngine.Random.Range(1, int.MaxValue);
        var initJob = new InitNoiseJob
        {
            States = cur,
            Width = width,
            Height = height,
            SolidBorder = _solidBorder ? (byte)1 : (byte)0,
            NoiseProbability = _noiseDensity / 100f,
            Seed = seed
        };
        var handle = initJob.Schedule(count, 2048);
        handle.Complete();

        for (int i = 0; i < _iterations; i++)
        {
            var stepJob = new StepJob
            {
                Cur = cur,
                Next = next,
                Width = width,
                Height = height,
                SolidBorder = _solidBorder ? (byte)1 : (byte)0,
                Birth = (byte)_birthThreshold,
                Survive = (byte)_surviveThreshold
            };
            handle = stepJob.Schedule(count, 4096);
            handle.Complete();

            var tmp = cur; cur = next; next = tmp;
        }

        // Convert to managed data for Unity application
        var managed = cur.ToArray();
        cur.Dispose();
        next.Dispose();

        int chunk = Mathf.Clamp(_applyChunkSize, 1, 256);
        int totalChunksX = Mathf.CeilToInt(width / (float)chunk);
        int totalChunksY = Mathf.CeilToInt(height / (float)chunk);

        // Progressive application
        for (int cy = 0; cy < totalChunksY; cy++)
        {
            for (int cx = 0; cx < totalChunksX; cx++)
            {
                int startX = cx * chunk;
                int startY = cy * chunk;
                int endX = Mathf.Min(startX + chunk, width);
                int endY = Mathf.Min(startY + chunk, height);

                // Apply this chunk to grid
                for (int y = startY; y < endY; y++)
                {
                    int rowStart = y * width;
                    for (int x = startX; x < endX; x++)
                    {
                        byte v = managed[rowStart + x];
                        if (!Grid.TryGetCellByCoordinates(x, y, out var cell)) continue;

                        if (v == 1)
                            AddTileToCell(cell, GRASS_TILE_NAME, true);
                        else
                            AddTileToCell(cell, WATER_TILE_NAME, true);
                    }
                }

                // Allow frame to breathe between chunks
                await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
            }
        }
    }

    [BurstCompile]
    private struct InitNoiseJob : IJobParallelFor
    {
        public NativeArray<byte> States;
        public int Width;
        public int Height;
        public byte SolidBorder;
        public float NoiseProbability;
        public uint Seed;

        public void Execute(int index)
        {
            int x = index % Width;
            int y = index / Width;

            if (SolidBorder == 1 && (x == 0 || y == 0 || x == Width - 1 || y == Height - 1))
            {
                States[index] = 1;
                return;
            }

            uint hash = (uint)(index * 747796405u) ^ Seed;
            hash ^= hash >> 16;
            hash *= 2246822519u;
            hash ^= hash >> 13;
            hash *= 3266489917u;
            hash ^= hash >> 16;
            float r = (hash & 0x00FFFFFF) / 16777216f;

            States[index] = (byte)(r < NoiseProbability ? 1 : 0);
        }
    }

    [BurstCompile]
    private struct StepJob : IJobParallelFor
    {
        [ReadOnly] public NativeArray<byte> Cur;
        public NativeArray<byte> Next;
        public int Width;
        public int Height;
        public byte SolidBorder;
        public byte Birth;
        public byte Survive;

        public void Execute(int index)
        {
            int x = index % Width;
            int y = index / Width;

            if (SolidBorder == 1 && (x == 0 || y == 0 || x == Width - 1 || y == Height - 1))
            {
                Next[index] = 1;
                return;
            }

            int wallCount = 0;
            for (int oy = -1; oy <= 1; oy++)
            {
                int ny = y + oy;
                if (SolidBorder == 1)
                {
                    if (ny < 0 || ny >= Height) { wallCount += 3; continue; }
                }
                else
                {
                    if (ny < 0) ny += Height;
                    else if (ny >= Height) ny -= Height;
                }

                for (int ox = -1; ox <= 1; ox++)
                {
                    if (ox == 0 && oy == 0) continue;
                    int nx = x + ox;

                    if (SolidBorder == 1)
                    {
                        if (nx < 0 || nx >= Width) { wallCount++; continue; }
                    }
                    else
                    {
                        if (nx < 0) nx += Width;
                        else if (nx >= Width) nx -= Width;
                    }

                    int nIdx = ny * Width + nx;
                    wallCount += Cur[nIdx];
                }
            }

            byte current = Cur[index];
            if (current == 1)
                Next[index] = (byte)(wallCount >= Survive ? 1 : 0);
            else
                Next[index] = (byte)(wallCount >= Birth ? 1 : 0);
        }
    }
}
