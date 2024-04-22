using UnityEngine;
using UnityEngine.UI;

namespace GameMain
{
    public class ShotGunHUD : WeaponHUD
    {
        public override float ChargePercent
        {
            get => m_HUDImage.fillAmount;
            set { m_HUDImage.fillAmount = value; }
        }

        protected float m_MinAngle;
        protected float m_MaxAngle;
        protected float[] m_ChargePercentList;

        protected Color[] m_ChargeColorList = new[]
        {
            new Color(0, 1, 0, 0.15f), new Color(0.33f, 0.66f, 0, 0.15f),
            new Color(0.66f, 0.33f, 0, 0.15f), new Color(1, 0, 0, 0.15f)
        };

        public override void Charge(float percent)
        {
            for (int i = 3; i >= 0; i--)
                if (percent > m_ChargePercentList[i])
                {
                    m_HUDImage.color = m_ChargeColorList[i];
                    break;
                }

            percent = (m_MinAngle + (m_MaxAngle - m_MinAngle) * percent) / 360f;

            ChargePercent = percent;
        }

        public void Init(float min, float max, float[] chargePercentList, Vector2 muzzle)
        {
            m_HUDImage = GetComponent<Image>();
            m_MinAngle = min;
            m_MaxAngle = max;
            m_ChargePercentList = chargePercentList;
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