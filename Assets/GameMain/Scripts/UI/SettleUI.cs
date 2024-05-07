using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameMain.Scripts.UI
{
    public class SettleUI : MonoBehaviour
    {
        [SerializeField] private Sprite m_NoStarImage;
        [SerializeField] private Sprite m_StarImage;
        private Button m_ReturnButton;

        private List<Image> stars = new();
        private List<TextMeshProUGUI> texts = new();

        private static Dictionary<int, List<string>> textDict = new()
        {
            {
                1, new List<string>() { $"分数达到{GameBase.scoreDict[1]}分", "未受到怪物攻击" }
            },
            {
                2, new List<string>() { $"分数达到{GameBase.scoreDict[2]}分", "未触发魔法阵" }
            },
            {
                3, new List<string>() { $"分数达到{GameBase.scoreDict[3]}分", "未受到火焰攻伤害" }
            }
        };

        private void Awake()
        {
            for (int i = 1; i <= 3; i++)
            {
                var trans = transform.Find($"Stars/{i.ToString()}");
                stars.Add(trans.GetComponent<Image>());
                texts.Add(trans.GetComponentInChildren<TextMeshProUGUI>());
            }

            m_ReturnButton = transform.Find("Return").GetComponent<Button>();
            m_ReturnButton.onClick.AddListener(GameBase.Instance.ReturnMenu);
            texts[0].text = "通过本关";
            texts[1].text = textDict[GameBase.Instance.Level][0];
            texts[2].text = textDict[GameBase.Instance.Level][1];
        }

        public void SetStar(List<bool> bools)
        {
            for (int i = 1; i <= 2; i++)
            {
                stars[i].sprite = bools[i] ? m_StarImage : m_NoStarImage;
            }

            int star = 0;
            foreach (var b in bools)
            {
                if (b) star++;
            }

            GameEntry.Setting.SetInt($"Level{GameBase.Instance.Level}Score", star);
        }
    }
}