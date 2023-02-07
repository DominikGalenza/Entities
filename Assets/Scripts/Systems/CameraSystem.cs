using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile]
[UpdateInGroup(typeof(LateSimulationSystemGroup))]
partial struct CameraSystem : ISystem
{
    Entity target;
    Random random;
    EntityQuery tanksQuery;

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        random = Random.CreateFromIndex(1234);
        tanksQuery = SystemAPI.QueryBuilder().WithAll<Tank>().Build();
        state.RequireForUpdate(tanksQuery);
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
    }

    public void OnUpdate(ref SystemState state)
    {
        if (target == Entity.Null || UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.Space))
        {
            NativeArray<Entity> tanks = tanksQuery.ToEntityArray(Allocator.Temp);
            target = tanks[random.NextInt(tanks.Length)];
        }

        UnityEngine.Transform cameraTransform = CameraSingleton.Instance.transform;
        var tankTransform = SystemAPI.GetComponent<LocalToWorld>(target);
        cameraTransform.position = tankTransform.Position - 10f * tankTransform.Forward + new float3(0f, 5f, 0f);
        cameraTransform.LookAt(tankTransform.Position, new float3(0f, 1f, 0f));
    }
}
