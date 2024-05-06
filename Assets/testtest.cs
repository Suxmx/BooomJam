using System;
using System.Collections;
using System.Collections.Generic;
using GameMain;
using UnityEngine;
using UnityEngine.UI;

public class testtest : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            var procedure = GameEntry.Procedure.CurrentProcedure;
            (procedure as ProcedureLevelSelect).SelectLevel(1);
        });
    }
}