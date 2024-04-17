using UnityEngine;
using UnityEngine.UI;

namespace GameMain
{
    public class ShotGunHUD : WeaponHUD
    {
        public override float ChargePercent
        {
            get => m_HUDImage.fillAmount;
            set => m_HUDImage.fillAmount = value;
        }

        public override void Charge(float percent)
        {
            ChargePercent = percent;
        }

        public override void Init()
        {
            m_HUDImage = GetComponent<Image>();
        }

        public override void SetDirection(Vector2 direction)
        {
            base.SetDirection(direction);
            float offsetAngle = ChargePercent * 360 / 2;
            transform.right = Quaternion.AngleAxis(offsetAngle, Vector3.forward) * direction;
        }
    }
}