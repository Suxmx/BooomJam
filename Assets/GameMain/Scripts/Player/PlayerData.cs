using System.Collections.Generic;

namespace GameMain
{
    public class PlayerData : EntityData
    {
        public int MaxHp;
        public int Damage;
        public float MoveSpeed;
        public float ChangeSceneInterval;
        public float InvincibleTime;
        public List<WeaponData> WeaponsDatas;
        
        public PlayerData(int entityId, int typeId, int maxHp, int damage, float moveSpeed,float changeSceneInterval, List<WeaponData> weaponsDatas) : base(entityId, typeId)
        {
            MaxHp = maxHp;
            Damage = damage;
            MoveSpeed = moveSpeed;
            ChangeSceneInterval = changeSceneInterval;
            WeaponsDatas = weaponsDatas;
        }
    }
}