using Unity.Entities;

class TurretAuthoring : UnityEngine.MonoBehaviour
{
    class TurretBaker : Baker<TurretAuthoring>
    {
        public override void Bake(TurretAuthoring authoring)
        {
            AddComponent<Turret>();
        }
    }
}

struct Turret : IComponentData
{
}