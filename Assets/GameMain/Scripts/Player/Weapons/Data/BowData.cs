namespace GameMain
{
    public class BowData : WeaponData
    {
        public float MinArrowAliveTime;
        public float MaxArrowAliveTime;
        public float MaxRandomAngle;
        public float MinRandomAngle;
        public float BulletSpeed;
        
        public BowData(int entityId, int typeId, DRWeapon drWeapon) : base(entityId, typeId, drWeapon)
        {
            Damage = drWeapon.IntParams[0];
            MaxChargeTime = drWeapon.FloatParams[0];
            FireInterval = drWeapon.FloatParams[1];
            MinArrowAliveTime = drWeapon.FloatParams[2];
            MaxArrowAliveTime = drWeapon.FloatParams[3];
            MinRandomAngle = drWeapon.FloatParams[4];
            MaxRandomAngle = drWeapon.FloatParams[5];
            BulletSpeed = drWeapon.FloatParams[6];
        }
    }
}