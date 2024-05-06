using System;
using UnityEngine;
using UnityEngine.UI;

namespace GameMain.Scripts.UI
{
    public class MenuButtons : MonoBehaviour
    {
        [SerializeField] private MenuVideo Video;

        private void Awake()
        {
            transform.Find("Return").GetComponent<Button>().onClick.AddListener(() => { Video.Replay(); });
            transform.Find("Exit").GetComponent<Button>().onClick.AddListener(Exit);
            transform.Find("ReturnMenu").GetComponent<Button>().onClick.AddListener(() => { Video.Replay(); });
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