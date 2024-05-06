using UnityEngine;
using UnityEngine.UI;

namespace GameMain.Scripts.UI
{
    public class MainUI : MonoBehaviour
    {
        private void Awake()
        {
            transform.Find("Return").GetComponent<Button>().onClick.AddListener(() => {  });
            transform.Find("Exit").GetComponent<Button>().onClick.AddListener(Exit);
            transform.Find("ReturnMenu").GetComponent<Button>().onClick.AddListener(() => {  });
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