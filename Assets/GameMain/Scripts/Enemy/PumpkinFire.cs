using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework.Fsm;
using Pathfinding;
using UnityEngine;
using UnityGameFramework.Runtime;
using MyTimer;

namespace GameMain
{
    public class PumpkinFire : Enemy
    {   
        private CountdownTimer m_timer;

        public void Init()
        {
            float Lasttime = 10f;
            m_timer = new CountdownTimer();
            m_timer.OnComplete += OnTimerComplete;
            m_timer.OnTick += Timer;
            m_timer.Initialize(Lasttime, true);
        }
        
        public override void GetBeaten(Vector2 force)
        {
            
        }

        public void Timer(float t)
        {
            //Log.Info(t);
        }
        public void OnTimerComplete()
        {
            RecycleSelf();
            m_timer.Paused = true;
        }
    }
}
