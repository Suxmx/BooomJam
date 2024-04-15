using System;

namespace GameMain
{
    /// <summary>
    /// 在游戏流程中打包给武器的数据包
    /// </summary>
    public abstract class WeaponData : EntityData
    {
        public int Damage;
        public float FireInterval;
        public float MaxChargeTime;
        public Type LogicType;

        public WeaponData(int entityId, int typeId, int damage, float fireInterval, Type logicType) : base(entityId,
            typeId)
        {
            Damage = damage;
            FireInterval = fireInterval;
            LogicType = logicType;
        }

        public WeaponData(int entityId, int typeId, DRWeapon drWeapon) : base(entityId, typeId)
        {
            LogicType = drWeapon.LogicType;
        }
    }

    public static class WeaponFactory
    {
        public static WeaponData GetWeaponData(DRWeapon drWeapon)
        {
            if (drWeapon.LogicType == typeof(ShotGun))
            {
                return new ShotGunData(GameEntry.Entity.GenerateSerialId(), (int)EWeapon.ShotGun, drWeapon);
            }
            else //BOW
            {
                return new BowData(GameEntry.Entity.GenerateSerialId(), (int)EWeapon.Bow, drWeapon);
            }
        }
    }
}