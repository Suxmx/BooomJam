using System;
using System.Collections;
using System.Collections.Generic;
using GameMain;
using GameMain.Scripts.UI;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour
{
    private int m_SelectedIndex = -1;
    private List<Button> m_LevelButtons = new();
    

    private void Start()
    {
        for (int i = 1; i <= 3; i++)
        {
            Button btn = transform.Find($"Level{i.ToString()}").GetComponent<Button>();
            var i1 = i;
            btn.onClick.AddListener(() => { SelectLevel(i1, btn); });
            m_LevelButtons.Add(btn);
            btn.GetComponent<LevelButton>().SetStar(GameEntry.Setting.GetInt($"Level{i.ToString()}Score"));
        }
    }

    private void SelectLevel(int index, Button btn)
    {
        m_SelectedIndex = index;
        var procedure = GameEntry.Procedure.CurrentProcedure;
        (procedure as ProcedureLevelSelect).SelectLevel(m_SelectedIndex);

    }
    
}