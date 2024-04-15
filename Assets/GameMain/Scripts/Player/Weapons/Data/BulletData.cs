using UnityEngine;

namespace GameMain
{
    public class BulletData : EntityData
    {
        public bool ThroughAble;
        public int Damage;
        public float Speed;
        public float AliveTime;
        public float ScaleFactor = 1;
        public Vector2 Direction;
        public Player Player;

        public BulletData(int entityId, int typeId, bool throughAble, int damage, float speed, float aliveTime,
            float scaleFactor) : base(entityId, typeId)
        {
            ThroughAble = throughAble;
            Damage = damage;
            Speed = speed;
            AliveTime = aliveTime;
            ScaleFactor = scaleFactor;
        }

        public BulletData(int entityId, int typeId, bool throughAble, int damage, float speed, float aliveTime,
            Vector2 direction) : base(entityId, typeId)
        {
            ThroughAble = throughAble;
            Damage = damage;
            Speed = speed;
            AliveTime = aliveTime;
            Direction = direction;
            ScaleFactor = 1;
        }

        public BulletData(int entityId, int typeId, bool throughAble, int damage, float speed, float aliveTime) : base(
            entityId, typeId)
        {
            ThroughAble = throughAble;
            Damage = damage;
            Speed = speed;
            AliveTime = aliveTime;
            ScaleFactor = 1;
        }
    }
}