using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameMain.Scripts.Player.Weapons.ObjectPool;

namespace GameMain
{
    public class Pumpkin : Enemy
    {   
        public bool isRed;
        public bool isBlue;
        private bool invincible;
        private bool isInGameScene;
        private int m_OwnerGameSceneIndex;
        private int m_CurrentSceneIndex;
        protected PublicObjectPool m_PublicObjectPool;
        
        public override void OnAttacked(AttackData data)
        {
            if (invincible) return;
            base.OnAttacked(data);
        }

        public void Init(int ownerIndex,int currentIndex)
        {
            m_OwnerGameSceneIndex = ownerIndex;
            m_CurrentSceneIndex = currentIndex;
            m_PublicObjectPool = GameBase.Instance.GetObjectPool();
            
            if (m_OwnerGameSceneIndex == m_CurrentSceneIndex)
            {
                isInGameScene = true;
            }
            else
            {
                isInGameScene = false;
            }
        }

        public override void OnDead()
        {
            base.OnDead();
            if (isInGameScene)
            {
                
            }
        }
        
        public override void RecycleSelf()
        {
            if (recycled) return;
            if (isRed)
            {
                var fire = m_PublicObjectPool.Spawn("RedPumpkinFire");
                fire.transform.position = transform.position;
            }
            if (isBlue)
            {
                var fire = m_PublicObjectPool.Spawn("BluePumpkinFire");
                fire.transform.position = transform.position;
            }
            recycled = true;
            gameObject.SetActive(false);
        }
    }
}
