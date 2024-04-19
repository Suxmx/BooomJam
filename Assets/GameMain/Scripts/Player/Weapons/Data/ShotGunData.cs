using System;

namespace GameMain
{
    
    public class ShotGunData : WeaponData
    {
        public int MinBulletNumPerFire;
        public int MaxBulletNumPerFire;
        public float BulletIntervalAngle;
        public float BulletRandomAngle;
        public float BulletSpeed;
        public float ChargeScaleFactor;
        public float MinRecoilValue;
        public float MaxRecoilValue;
        public float Jump;
        
        public ShotGunData(int entityId, int typeId, DRWeapon drWeapon) : base(entityId, typeId, drWeapon)
        {
            Damage = drWeapon.IntParams[0];
            MinBulletNumPerFire = drWeapon.IntParams[1];
            MaxBulletNumPerFire = drWeapon.IntParams[2];

            FireInterval = drWeapon.FloatParams[0];
            BulletIntervalAngle = drWeapon.FloatParams[1];
            BulletRandomAngle = drWeapon.FloatParams[2];
            BulletSpeed = drWeapon.FloatParams[3];
            MaxChargeTime = drWeapon.FloatParams[4];
            ChargeScaleFactor = drWeapon.FloatParams[5];
            MinRecoilValue = drWeapon.FloatParams[6];
            MaxRecoilValue = drWeapon.FloatParams[7];
            Jump = drWeapon.FloatParams[8];
        }
    }
}