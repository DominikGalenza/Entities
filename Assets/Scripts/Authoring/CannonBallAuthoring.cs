using Unity.Entities;
using Unity.Rendering;
using Unity.Mathematics;

public class CannonBallAuthoring : UnityEngine.MonoBehaviour
{
    class CannonBallBaker : Baker<CannonBallAuthoring>
    {
        public override void Bake(CannonBallAuthoring authoring)
        {
            AddComponent<CannonBall>();
        }
    }
}

struct CannonBall : IComponentData
{
    public float3 Speed;
}