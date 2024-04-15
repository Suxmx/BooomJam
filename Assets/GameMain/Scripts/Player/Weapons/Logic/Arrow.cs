using System;
using UnityEngine;

namespace GameMain
{
    public class Arrow : Bullet
    {
        protected Player m_Player;
        public override void OnShow(object userData)
        {
            base.OnShow(userData);
            BulletData data = (BulletData)userData;
            m_Player = data.Player;
        }

        protected override void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Ground"))
            {
                RecycleSelf();
            }

            if (other.CompareTag("Enemy"))
            {
                if (other.TryGetComponent<IAttackable>(out var attackable))
                {
                    attackable.OnAttacked(new AttackData(m_Damage));
                }

                if (!m_ThroughAble) RecycleSelf();
            }
        }

        public override void RecycleSelf()
        {
            m_Player.Teleport(transform.position);
            base.RecycleSelf();
        }
    }
    
}