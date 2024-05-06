using System;
using UnityEngine;
using UnityEngine.UI;

namespace GameMain.Scripts.UI
{
    public class MenuButtons : MonoBehaviour
    {
        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(Exit);
        }

        private void Exit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}