using System;
using MyTimer;
using TMPro;
using UnityEngine;

namespace GameMain.Scripts.UI
{
    public class CountDownUI : MonoBehaviour
    {
        private TextMeshProUGUI m_Text;
        private CountdownTimer m_Timer;

        private void Awake()
        {
            m_Text = transform.Find("Text").GetComponent<TextMeshProUGUI>();
            m_Timer = new CountdownTimer();
            m_Timer.Initialize(90f);
            m_Timer.OnTick += ChangeText;
            m_Timer.OnComplete += OnCompleted;
        }

        private void ChangeText(float t)
        {
            int intT = (int)t;
            m_Text.text = intT.ToString();
        }

        private void OnCompleted()
        {
            GameBase.Instance.OnWin();
        }

        public void Pause()
        {
            m_Timer.Paused = true;
        }
    }
}