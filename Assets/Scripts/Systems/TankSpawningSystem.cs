using System.Drawing;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;

[BurstCompile]
partial struct TankSpawningSystem : ISystem
{
    EntityQuery baseColorQuery;

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Config>();

        baseColorQuery = state.GetEntityQuery(ComponentType.ReadOnly<URPMaterialPropertyBaseColor>());
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        Config config = SystemAPI.GetSingleton<Config>();

        Random random = Random.CreateFromIndex(1234);
        float hue = random.NextFloat();

        URPMaterialPropertyBaseColor RandomColor()
        {
            hue = (hue + 0.618034005f) % 1;
            UnityEngine.Color color = UnityEngine.Color.HSVToRGB(hue, 1f, 1f);
            return new URPMaterialPropertyBaseColor { Value = (UnityEngine.Vector4)color };
        }

        BeginSimulationEntityCommandBufferSystem.Singleton entityCommandBufferSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
        EntityCommandBuffer entityCommandBuffer = entityCommandBufferSingleton.CreateCommandBuffer(state.WorldUnmanaged);

        NativeArray<Entity> vehicles = CollectionHelper.CreateNativeArray<Entity>(config.TankCount, Allocator.Temp);
        entityCommandBuffer.Instantiate(config.TankPrefab, vehicles);

        EntityQueryMask queryMask = baseColorQuery.GetEntityQueryMask();

        foreach (Entity vehicle in vehicles)
        {
            entityCommandBuffer.SetComponentForLinkedEntityGroup(vehicle, queryMask, RandomColor());
        }

        state.Enabled = false;
    }
}
