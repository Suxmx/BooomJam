using System.Collections.Generic;

namespace GameMain
{
    public class PlayerData
    {
        public int MaxHp;
        public int Damage;
        public float MoveSpeed;
        public float ChangeSceneInterval;
        public float InvincibleTime = 0.3f;
        public List<WeaponData> WeaponsDatas;
        
        public PlayerData(int maxHp, int damage, float moveSpeed,float changeSceneInterval, List<WeaponData> weaponsDatas)
        {
            MaxHp = maxHp;
            Damage = damage;
            MoveSpeed = moveSpeed;
            ChangeSceneInterval = changeSceneInterval;
            WeaponsDatas = weaponsDatas;
        }
    }
}