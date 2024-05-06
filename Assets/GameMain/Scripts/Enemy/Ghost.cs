using UnityEngine;

namespace GameMain
{
    public class Ghost : Enemy
    {
        private bool invincible;
        private int m_OwnerGameSceneIndex;
        private bool ifHide;
        
        public override void OnAttacked(AttackData data)
        {
            if (invincible) return;
            base.OnAttacked(data);
        }

        public override void OnSpawnSuccess()
        {
            base.OnSpawnSuccess();
            if (ifHide) Hide();

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

            GameBase.Instance.OnChangeGameScene += OnGameSceneChange;
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
            gameObject.layer = LayerMask.NameToLayer("Enemy");
            m_SpriteRenderer.color = Color.white;
            invincible = false;
            ifHide = false;
        }

        private void Hide()
        {
            gameObject.layer = LayerMask.NameToLayer("Ghost");
            m_SpriteRenderer.color = new Color(1, 1, 1, 0.4f);
            invincible = true;
            ifHide = true;
        }
        
    }
}