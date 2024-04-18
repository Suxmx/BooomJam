using System;
using UnityEngine;
using UnityEngine.Pool;
using UnityGameFramework.Runtime;
using ObjectBase = GameFramework.ObjectPool.ObjectBase;

namespace GameMain
{
    public abstract class WeaponBase : EntityLogic
    {
        public int Damage { get; protected set; }

        protected Vector3 m_OriginalScale;
        protected Vector2 m_FireDirection;
        protected float m_FireInterval;
        protected float m_FireTimer;
        protected float m_ChargeTime;
        protected float m_MaxChargeTime;
        protected float m_BulletRandomAngle;
        protected float m_BulletSpeed;
        protected GameObject m_BulletTemplate;
        protected Transform m_Muzzle;
        protected WeaponHUD m_ChargeHUD;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            m_OriginalScale = transform.localScale;
        }

        public void Fire(Player player)
        {
            Fire(player,m_ChargeTime);
            m_ChargeTime = 0f;
        }
        public abstract void Fire(Player player, float chargeTime);

        protected  virtual void Update()
        {
            ChangeDirection();
        }

        protected void LateUpdate()
        {
            
        }

        public void ChangeDirection()
        {
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            m_FireDirection = mouseWorldPos - (Vector2)transform.position;
            m_FireDirection = m_FireDirection.normalized;
            transform.right = m_FireDirection;
            if (mouseWorldPos.x > transform.position.x)
            {
                transform.localScale = m_OriginalScale;
                m_ChargeHUD.Flip = false;
            }
            else
            {
                transform.localScale = new Vector3(m_OriginalScale.x, -m_OriginalScale.y, m_OriginalScale.z);
                m_ChargeHUD.Flip = true;
            }
        }

        public virtual void Charge(float deltaTime)
        {
            m_ChargeTime += deltaTime;
            m_ChargeTime = m_ChargeTime > m_MaxChargeTime ? m_MaxChargeTime : m_ChargeTime;
            m_ChargeHUD.Show();
            m_ChargeHUD.Charge(GetChargePercent());
            m_ChargeHUD.SetDirection(m_FireDirection);
        }

        public float GetChargePercent()
        {
            if (Mathf.Abs(m_ChargeTime - m_MaxChargeTime) < 1e-5) return 1;
            else return m_ChargeTime / m_MaxChargeTime;
        }
    }
}