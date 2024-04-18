using UnityEngine;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class BowHUD : WeaponHUD
    {
        private float m_MinLength;
        private float m_MaxLength;
        private float m_MinRandomAngle;
        private float m_MaxRandomAngle;

        public override float ChargePercent
        {
            get => m_HUDImage.transform.localScale.x;
            set
            {
                m_HUDImage.transform.localScale =
                    new Vector3((m_MinLength + (m_MaxLength - m_MinLength) * value),
                        m_MaxLength * Mathf.Tan((m_MaxRandomAngle - (m_MaxRandomAngle - m_MinRandomAngle) * value) / 2 /
                            180 * Mathf.PI) * 2, 1);
                if (Mathf.Abs(value - 1) < 1e-5)
                {
                    m_HUDImage.color = new Color(0, 1, 0, 0.15f);
                }
            }
        }


        public override void Charge(float percent)
        {
            ChargePercent = percent;
        }

        public void Init(float minLength, float maxLength, float minRandomAngle, float maxRandomAngle, Vector2 muzzle)
        {
            m_MinLength = minLength;
            m_MaxLength = maxLength;
            m_MinRandomAngle = minRandomAngle;
            m_MaxRandomAngle = maxRandomAngle;
            transform.position = muzzle;
        }
        
    }
}