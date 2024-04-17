using UnityEngine;
using UnityEngine.UI;

namespace GameMain
{
    public abstract class WeaponHUD : MonoBehaviour
    {
        protected Image m_HUDImage;

        public abstract float ChargePercent
        {
            get;
            set;
        }

        public abstract void Charge(float percent);

        public virtual void SetDirection(Vector2 direction)
        {
            transform.right = direction;
        }
        public abstract void Init();

        public void Show()
        {
            m_HUDImage.gameObject.SetActive(true);
        }

        public void Hide()
        {
            m_HUDImage.gameObject.SetActive(false);
        }

    }
}