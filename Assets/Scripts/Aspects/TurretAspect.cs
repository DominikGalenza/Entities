using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;

readonly partial struct TurretAspect : IAspect
{
    readonly RefRO<Turret> turret;
    readonly RefRO<URPMaterialPropertyBaseColor> baseColor;

    public Entity CannonBallSpawn => turret.ValueRO.CannonBallSpawn;
    public Entity CannonBallPrefab => turret.ValueRO.CannonBallPrefab;
    public float4 Color => baseColor.ValueRO.Value;
}
