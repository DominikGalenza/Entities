using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

partial class TankMovingSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float deltaTime = SystemAPI.Time.DeltaTime;

        Entities
            .WithAll<Tank>()
            .ForEach((Entity entity, TransformAspect transform) =>
            {
                float3 position = transform.LocalPosition;

                position.y = entity.Index;
                float angle = (0.5f + noise.cnoise(position / 10f)) * 4f * math.PI;

                float3 direction = float3.zero;
                math.sincos(angle, out direction.x, out direction.z);
                transform.LocalPosition += direction * deltaTime * 5f;
                transform.LocalRotation = quaternion.RotateY(angle);
            }).ScheduleParallel();
    }
}
