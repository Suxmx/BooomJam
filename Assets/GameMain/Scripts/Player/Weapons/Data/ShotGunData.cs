using System;

namespace GameMain
{
    public class ShotGunData : WeaponData
    {
        public int BulletNumPerFire;
        public float BulletIntervalAngle;
        public ShotGunData(int entityId, int typeId, DRWeapon drWeapon) : base(entityId, typeId, drWeapon)
        {
            Damage = drWeapon.IntParams[0];
            BulletNumPerFire = drWeapon.IntParams[1];

            FireInterval = drWeapon.FloatParams[0];
            BulletIntervalAngle = drWeapon.FloatParams[1];
        }
    }
}