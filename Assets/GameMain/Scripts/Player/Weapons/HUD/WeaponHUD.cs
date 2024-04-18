using System;
using UnityEngine;
using UnityEngine.UI;

namespace GameMain
{
    public abstract class WeaponHUD : MonoBehaviour
    {
        protected Image m_HUDImage;

        protected virtual void Awake()
        {
            m_HUDImage = GetComponent<Image>();
        }

        public abstract float ChargePercent
        {
            get;
            set;
        }

        public bool Flip;

        public abstract void Charge(float percent);

        public virtual void SetDirection(Vector2 direction)
        {
            transform.right = direction;
        }

        public void Show()
        {
            m_HUDImage.gameObject.SetActive(true);
        }

        public void Hide()
        {
            m_HUDImage.gameObject.SetActive(false);
            m_HUDImage.color = new Color(1, 0, 0, 0.15f);
        }
        

    }
}