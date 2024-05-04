using System;
using System.Collections;
using System.Collections.Generic;
using GameMain;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour
{
    private void Awake()
    {
        for (int i = 1; i <= 3; i++)
        {
            Button btn = transform.Find($"Level{i.ToString()}").GetComponent<Button>();
            var i1 = i;
            btn.onClick.AddListener(()=>{SelectLevel(i1);});
        }
    }

    private void SelectLevel(int index)
    {
        var procedure = GameEntry.Procedure.CurrentProcedure;
        (procedure as ProcedureLevelSelect).SelectLevel(index);
    }
}
