using System;
using System.Collections.Generic;

namespace GameMain
{
    
    public class ShotGunData : WeaponData
    {
        public int[] BulletNumPerFireList;
        public float[] BulletChargePercentList;
        public float BulletIntervalAngle;
        public float BulletRandomAngle;
        public float BulletSpeed;
        public float ChargeScaleFactor;
        public float MinRecoilValue;
        public float MaxRecoilValue;
        public float Jump;
        
        public ShotGunData(int entityId, int typeId, DRWeapon drWeapon) : base(entityId, typeId, drWeapon)
        {
            BulletNumPerFireList = new int[4];
            BulletChargePercentList = new float[4];
            Damage = drWeapon.IntParams[0];
            BulletNumPerFireList[0] = drWeapon.IntParams[1];
            BulletNumPerFireList[1] = drWeapon.IntParams[2];
            BulletNumPerFireList[2] = drWeapon.IntParams[3];
            BulletNumPerFireList[3] = drWeapon.IntParams[4];

            FireInterval = drWeapon.FloatParams[0];
            BulletIntervalAngle = drWeapon.FloatParams[1];
            BulletRandomAngle = drWeapon.FloatParams[2];
            BulletSpeed = drWeapon.FloatParams[3];
            MaxChargeTime = drWeapon.FloatParams[4];
            ChargeScaleFactor = drWeapon.FloatParams[5];
            MinRecoilValue = drWeapon.FloatParams[6];
            MaxRecoilValue = drWeapon.FloatParams[7];
            BulletChargePercentList[0] = -1f * 1e-3f;
            BulletChargePercentList[1] = drWeapon.FloatParams[8];
            BulletChargePercentList[2] = drWeapon.FloatParams[9];
            BulletChargePercentList[3] = drWeapon.FloatParams[10];

        }
    }
}