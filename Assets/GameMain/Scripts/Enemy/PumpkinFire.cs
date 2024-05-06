using System;
using System.Collections;
using UnityEngine;
using MyTimer;

namespace GameMain
{
    public class PumpkinFire : MonoBehaviour
    {   
        public bool isRed;
        public bool isBlue;
        private int m_CurrentSceneIndex;
        private CountdownTimer m_timer;
        private PublicObjectPool m_Pool;

        private void OnDestroy()
        {
            m_timer.Paused = true;
        }

        public void Init()
        {
            float Lasttime = 10f;
            m_timer = new CountdownTimer();
            m_timer.OnComplete += OnTimerComplete;
            m_timer.Initialize(Lasttime, true);
            m_CurrentSceneIndex = GameBase.Instance.GetGameSceneIndex();
            OnGameScene(m_CurrentSceneIndex);
            GameBase.Instance.OnChangeGameScene += OnGameScene;
            m_Pool = GameBase.Instance.GetObjectPool();
        }
        
        public void OnTimerComplete()
        {
            m_Pool.UnSpawn(gameObject);
            m_timer.Paused = true;
        }

        public void OnGameScene(int index)
        {
            if (isRed)
            {
                if (index == 0)
                {
                    gameObject.SetActive(true);
                }

                if (index == 1)
                {
                    gameObject.SetActive(false);
                }
            }
            if (isBlue)
            {
                if (index == 0)
                {
                    gameObject.SetActive(false);
                }

                if (index == 1)
                {
                    gameObject.SetActive(true);
                }
            }
            
        }
    }
}
