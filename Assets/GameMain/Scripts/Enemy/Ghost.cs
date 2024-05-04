using UnityEngine;

namespace GameMain
{
    public class Ghost : Enemy
    {
        private bool invincible;
        private int m_OwnerGameSceneIndex;
        
        public override void OnAttacked(AttackData data)
        {
            if (invincible) return;
            base.OnAttacked(data);
        }

        public void Init(int ownerIndex,int currentIndex)
        {
            m_OwnerGameSceneIndex = ownerIndex;
            if (ownerIndex == currentIndex)
            {
                Show();
            }
            else
            {
                Hide();
            }
        }

        private void OnGameSceneChange(int index)
        {
            if (index == m_OwnerGameSceneIndex)
            {
                Show();
            }
            else
            {
                Hide();
            }
        }

        private void Show()
        {
            m_SpriteRenderer.color = Color.white;
            invincible = false;
        }

        private void Hide()
        {
            m_SpriteRenderer.color = new Color(1, 1, 1, 0.4f);
            invincible = true;
        }
    }
}