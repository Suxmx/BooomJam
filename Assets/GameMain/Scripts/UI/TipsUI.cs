using System;
using System.Collections;
using UnityEngine;

namespace GameMain.Scripts.UI
{
    public class TipsUI : MonoBehaviour
    {
        public void Show()
        {
            StartCoroutine(nameof(IWait));
            transform.localScale = Vector3.one;
            Time.timeScale = 0f;
        }

        IEnumerator IWait()
        {
            float timer = 0f;
            while (timer < 3f)
            {
                timer += 0.3f;
                yield return new WaitForSecondsRealtime(0.3f);
            }

            Time.timeScale = 1;
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            Time.timeScale = 1;
        }
    }
}