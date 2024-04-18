using UnityEngine;
using UnityEngine.UI;

namespace GameMain
{
    public class ShotGunHUD : WeaponHUD
    {
        public override float ChargePercent
        {
            get => m_HUDImage.fillAmount;
            set
            {
                m_HUDImage.fillAmount = value;
                
            }
        }

        protected float m_MinAngle;
        protected float m_MaxAngle;

        public override void Charge(float percent)
        {
            if (Mathf.Abs(percent - 1) < 1e-5)
            {
                m_HUDImage.color = new Color(0, 1, 0, 0.15f);
            }
            percent = (m_MinAngle + (m_MaxAngle - m_MinAngle) * percent) / 360f;
            
            ChargePercent = percent;
        }

        public void Init(float min, float max, Vector2 muzzle)
        {
            m_HUDImage = GetComponent<Image>();
            m_MinAngle = min;
            m_MaxAngle = max;
            transform.position = muzzle;
            Hide();
        }

        public override void SetDirection(Vector2 direction)
        {
            base.SetDirection(direction);
            float offsetAngle = ChargePercent * 360 / 2;
            offsetAngle = Flip ? -offsetAngle : offsetAngle;
            transform.right = Quaternion.AngleAxis(offsetAngle, Vector3.forward) * direction;
        }
    }
}