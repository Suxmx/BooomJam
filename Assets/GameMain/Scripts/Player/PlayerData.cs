using System.Collections.Generic;

namespace GameMain
{
    public class PlayerData : EntityData
    {
        public int MaxHp;
        public int Damage;
        public float MoveSpeed;
        public List<WeaponData> WeaponsDatas;
        
        public PlayerData(int entityId, int typeId, int maxHp, int damage, float moveSpeed, List<WeaponData> weaponsDatas) : base(entityId, typeId)
        {
            MaxHp = maxHp;
            Damage = damage;
            MoveSpeed = moveSpeed;
            WeaponsDatas = weaponsDatas;
        }
    }
}