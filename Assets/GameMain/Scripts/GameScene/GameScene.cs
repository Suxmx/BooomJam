using System;
using System.Collections.Generic;
using MyTimer;
using UnityEngine;

namespace GameMain
{
    [Serializable]
    public class FMagicCircleData
    {
        public MagicCircle circle;
        public float ShowTime;
        [NonSerialized] public bool hasShowed;

        public FMagicCircleData(MagicCircle circle, float showTime)
        {
            this.circle = circle;
            ShowTime = showTime;
            this.hasShowed = false;
        }
    }

    public class GameScene : MonoBehaviour
    {
        public List<FMagicCircleData> MagicCircleDatas;
        private float m_Timer = 0f;


        public void OnChangeSceneToAnother()
        {
            gameObject.SetActive(false);
        }

        public void OnChangeSceneToThis()
        {
            gameObject.SetActive(true);
        }
    }
}