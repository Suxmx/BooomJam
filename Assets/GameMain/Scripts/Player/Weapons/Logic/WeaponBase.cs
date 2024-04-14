using UnityGameFramework.Runtime;

namespace GameMain
{
    public abstract class WeaponBase : EntityLogic
    {
        public int Damage
        {
            get;
            private set;
        }

        public abstract void Fire(Player player);
    }
}