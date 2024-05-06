using System;
using System.Collections;
using System.Collections.Generic;
using GameMain;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour
{
    [SerializeField] private Button StartButton;
    private int m_SelectedIndex = -1;
    private List<Button> m_LevelButtons=new();

    private void Awake()
    {
        for (int i = 1; i <= 3; i++)
        {
            Button btn = transform.Find($"Level{i.ToString()}").GetComponent<Button>();
            var i1 = i;
            btn.onClick.AddListener(() => { SelectLevel(i1, btn); });
            m_LevelButtons.Add(btn);
        }
        StartButton.onClick.AddListener(OnClickStart);
    }

    private void SelectLevel(int index, Button btn)
    {
        m_SelectedIndex = index;
        foreach (var b in m_LevelButtons)
        {
            b.GetComponent<Outline>().enabled = false;
        }
        btn.GetComponent<Outline>().enabled = true;
    }

    private void OnClickStart()
    {
        if (m_SelectedIndex == -1) return;
        var procedure = GameEntry.Procedure.CurrentProcedure;
        (procedure as ProcedureLevelSelect).SelectLevel(m_SelectedIndex);
    }
}