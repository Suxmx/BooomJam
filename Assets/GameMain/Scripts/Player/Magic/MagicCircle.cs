using System;
using System.Collections;
using System.Collections.Generic;
using GameMain;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace MyTimer
{
    public class MagicCircle : MonoBehaviour
    {
        public int OwnerGameSceneIndex;
        public float timeLimit;
        public float radius;
        public float directionX;
        public float directionY;
        private float m_fillAmount;
        private bool m_CanBeTriggered;
        public SpriteRenderer spriteRenderer;
        private CountdownTimer m_timer;
        private Vector2 m_magicCirclePosition;
        private Material m_material;


        private void OnDestroy()
        {
            m_timer.Paused = true;
            GameBase.Instance.OnChangeGameScene -= OnGameSceneChange;
        }

        private void Start()
        {
            m_timer = new CountdownTimer();
            m_timer.OnComplete += OnTimerComplete;
            m_timer.OnTick += CircleChange;
            m_timer.Initialize(10, true);

            float newScale = radius;
            transform.localScale = new Vector2(newScale, newScale);

            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            m_material = spriteRenderer.material;
            GameBase.Instance.OnChangeGameScene += OnGameSceneChange;
            OnGameSceneChange(GameBase.Instance.GetGameSceneIndex()); //防止出现的时候出错
        }

        private void OnGameSceneChange(int index)
        {
            if (index == OwnerGameSceneIndex)
            {
                spriteRenderer.color = new Color(1, 1, 1, 1);
                m_CanBeTriggered = true;
            }
            else
            {
                spriteRenderer.color = new Color(1, 1, 1, 0.5f);
                m_CanBeTriggered = false;
            }
        }


        private void CircleChange(float p)
        {
            if (m_timer != null && m_material != null)
            {
                float remainingPercent = m_timer.Percent;
                m_fillAmount = Mathf.Clamp01(1 - remainingPercent);
                m_material.SetFloat("_Fill", m_fillAmount);
            }
        }

        private void OnTriggerStay2D(Collider2D other) //防止切换场景时玩家踩在魔法阵上面
        {
            if (!m_CanBeTriggered) return;
            if (other.CompareTag("Player"))
            {
                if (other.GetComponent<Player>().m_IsMovingToward) return;
                m_CanBeTriggered = false;
                Destroy(gameObject);
            }
        }


        private void OnTimerComplete()
        {
            GameBase.Instance.GetSpawner().SpawnPlentyEnemy(10);
            GameBase.Instance.NoMagicCircleTriggered = false;
            Destroy(gameObject);
        }
    }
}