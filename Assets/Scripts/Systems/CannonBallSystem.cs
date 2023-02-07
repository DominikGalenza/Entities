using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

[BurstCompile]
partial struct CannonBallJob : IJobEntity
{
    public EntityCommandBuffer.ParallelWriter ECB;
    public float DeltaTime;

    void Execute([ChunkIndexInQuery] int chunkIndex, ref CannonBallAspect cannonBall)
    {
        float3 gravity = new float3(0f, -9.82f, 0f);
        float3 invertY = new float3(1f, -1f, 1f);

        cannonBall.Position += cannonBall.Speed * DeltaTime;
        if (cannonBall.Position.y < 0f)
        {
            cannonBall.Position *= invertY;
            cannonBall.Speed *= invertY * 0.8f;
        }

        cannonBall.Speed += gravity * DeltaTime;

        float speed = math.lengthsq(cannonBall.Speed);
        if (speed < 0.1f)
        {
            ECB.DestroyEntity(chunkIndex, cannonBall.Self);
        }
    }
}

[BurstCompile]
partial struct CannonBallSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EndSimulationEntityCommandBufferSystem.Singleton entityCommandBufferSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
        EntityCommandBuffer entityCommandBuffer = entityCommandBufferSingleton.CreateCommandBuffer(state.WorldUnmanaged);
        CannonBallJob cannonBallJob = new CannonBallJob
        {
            ECB = entityCommandBuffer.AsParallelWriter(),
            DeltaTime = SystemAPI.Time.DeltaTime
        };
        cannonBallJob.ScheduleParallel();
    }
}