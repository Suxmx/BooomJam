using System;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class Bullet : MonoBehaviour,IMyObject
    {
        protected bool m_ThroughAble;
        protected int m_Damage;
        protected float m_Speed;
        protected Vector2 m_Direction;
        protected float m_AliveTime;
        protected float m_AliveTimer;
        protected Vector3 m_OriginalScale;

        public virtual void OnInit(object userData)
        {
            m_OriginalScale = transform.localScale;
        }

        public virtual void OnShow(object userData)
        {
            BulletData data = (BulletData)userData;
            m_ThroughAble = data.ThroughAble;
            m_Damage = data.Damage;
            m_Speed = data.Speed;
            m_Direction = data.Direction;
            m_AliveTime = data.AliveTime;
            transform.position = data.Position;
            transform.localScale = m_OriginalScale * data.ScaleFactor;
            
            transform.right = m_Direction;
        }

        protected virtual void Update()
        {
            m_AliveTimer += Time.deltaTime;
            transform.Translate(Vector2.right * (m_Speed * Time.deltaTime));
            if (m_AliveTimer > m_AliveTime)
            {
                RecycleSelf();
            }
        }

        protected virtual void OnTriggerEnter2D(Collider2D other)
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

        public Action<object> RecycleAction { get; set; }

        public virtual void RecycleSelf()
        {
            RecycleAction(this);
            m_AliveTimer = 0;
            gameObject.SetActive(false);
        }
    }
}