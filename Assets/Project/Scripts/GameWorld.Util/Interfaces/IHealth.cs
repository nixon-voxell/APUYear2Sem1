using Unity.Mathematics;

namespace GameWorld.Util
{
    public interface IHealth
    {
        public float MaxHealth { get; }
        public float Health { get; }

        void SetHealth(float health);

        void ReduceHealth(float reduction)
        {
            SetHealth(math.max(this.Health - reduction, 0.0f));
        }

        void IncreaseHealth(float increment)
        {
            SetHealth(math.min(this.Health + increment, this.MaxHealth));
        }
    }
}
