using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;

readonly partial struct TurretAspect : IAspect
{
    readonly RefRO<Turret> turret;

    public Entity CannonBallSpawn => turret.ValueRO.CannonBallSpawn;
    public Entity CannonBallPrefab => turret.ValueRO.CannonBallPrefab;
}
