using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace MyTimer
{
    public class MagicCircle : EntityLogic
    {
        public float timeLimit;
        public float radius;
        public float directionX;
        public float directionY;
        public Transform player;
        public Image magicCircle;
        private CountdownTimer m_timer;
        private Vector2 m_magicCirclePosition;
        
        
        private void Start()
        {
            m_magicCirclePosition = new Vector2(directionX,directionY);
            
            m_timer = new CountdownTimer();
            m_timer.OnComplete += OnTimerComplete;
            m_timer.OnTick += CircleChange;
            m_timer.Initialize(timeLimit, true);
            
            float newScale = radius;
            transform.localScale = new Vector2(newScale, newScale);
        }

        private void Update()
        {
            if (!m_timer.Completed)
            {
                if (IsPlayerInMagicCircle())
                {
                    //m_timer.Restart();
                    OnTimerComplete();
                }
            }
            else
            {
                OnTimerComplete();
            }
        }

        private void CircleChange(float p)
        {
            Log.Info(p);
            float remainingAngle = 360f * (1 - m_timer.Percent);
            if (magicCircle != null)
            {
                magicCircle.fillAmount = remainingAngle / 360f;
            }
        }
        
        private bool IsPlayerInMagicCircle()
        {
            float distance = Vector2.Distance(player.position, m_magicCirclePosition);
            return distance <= radius;
        }
        
        private void OnTimerComplete()
        {
            Destroy(gameObject);
            m_timer.Paused = true;
        }
    }
}
