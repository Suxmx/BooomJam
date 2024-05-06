using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace MyTimer
{
    public class MagicCircle : MonoBehaviour
    {
        public float timeLimit;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           
        public float radius;
        public float directionX;
        public float directionY;
        private float m_fillAmount;
        private bool m_isPlayerInside = false;
        public Transform player;
        public  SpriteRenderer spriteRenderer;
        private CountdownTimer m_timer;
        private Vector2 m_magicCirclePosition;
        private Material m_material;


        private void OnDestroy()
        {
            m_timer.Paused = true;
        }

        private void Start()
        {
            m_magicCirclePosition = new Vector2(directionX,directionY);
            transform.position = m_magicCirclePosition;
            
            m_timer = new CountdownTimer();
            m_timer.OnComplete += OnTimerComplete;
            m_timer.OnTick += CircleChange;
            m_timer.Initialize(timeLimit, true);
            
            float newScale = radius;
            transform.localScale = new Vector2(newScale, newScale);
            
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            m_material = spriteRenderer.material;
        }

        private void Update()
        {
            if (!m_timer.Completed)
            {   
                if (m_isPlayerInside)
                {   
                    Debug.Log("In");
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
            //Log.Info(p);
            if (m_timer != null && m_material != null)
            {
                float remainingPercent = m_timer.Percent;
                m_fillAmount = Mathf.Clamp01(1 - remainingPercent);
                m_material.SetFloat("_Fill", m_fillAmount);
            } 
            //Log.Info(m_fillAmount);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                m_isPlayerInside = true;
            }
        }


        private void OnTimerComplete()
        {
            Destroy(gameObject);
            m_timer.Paused = true;
        }
    }
}
