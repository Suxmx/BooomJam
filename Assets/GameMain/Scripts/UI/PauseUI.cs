using System;
using UnityEngine;
using UnityEngine.UI;

namespace GameMain.Scripts.UI
{
    public class PauseUI : MonoBehaviour
    {
        private Button m_Exit;
        private Button m_Home;
        private Button m_Continue;

        private void Awake()
        {
            var parent = transform.Find("Buttons");
            m_Exit = parent.Find("Exit").GetComponent<Button>();
            m_Home = parent.Find("Home").GetComponent<Button>();
            m_Continue = parent.Find("Continue").GetComponent<Button>();
            m_Exit.onClick.AddListener(Exit);
            m_Home.onClick.AddListener(GameBase.Instance.ReturnMenu);
            m_Continue.onClick.AddListener(()=>{GameBase.Instance.ContinueGame();});
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