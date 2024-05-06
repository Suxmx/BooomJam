using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameMain.Scripts.UI
{
    public class HpUI : MonoBehaviour
    {
        [SerializeField] private Sprite EmptyHeart;
        [SerializeField] private Sprite Heart;
        private List<Image> m_HpUnits=new();

        private void Awake()
        {
            for (int i = 1; i <= 5; i++)
            {
                m_HpUnits.Add(transform.Find($"HpUnit{i.ToString()}").GetComponent<Image>());
            }
        }

        public void SetHp(int hp)
        {
            if (hp < 0 || hp > 5) return;
            for (int i = 0; i < hp; i++)
            {
                m_HpUnits[i].sprite = Heart;
            }

            for (int i = hp; i < 5; i++)
            {
                m_HpUnits[i].sprite = EmptyHeart;
            }
        }
    }
}