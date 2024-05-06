using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameMain.Scripts.UI
{
    public class LevelButton : MonoBehaviour
    {
        [SerializeField] private Sprite NoStarSprite;
        [SerializeField] private Sprite StarSprite;
        private List<Image> m_StarSrs = new();

        private void Awake()
        {
            for (int i = 1; i <= 3; i++)
                m_StarSrs.Add(transform.Find($"StarPositions/{i.ToString()}").GetComponent<Image>());
        }

        public void SetStar(int num)
        {
            for (int i = 1; i <= num; i++)
            {
                m_StarSrs[i - 1].sprite = StarSprite;
            }

            for (int i = num + 1; i <= 3; i++)
            {
                m_StarSrs[i - 1].sprite = NoStarSprite;
            }
        }
    }
}