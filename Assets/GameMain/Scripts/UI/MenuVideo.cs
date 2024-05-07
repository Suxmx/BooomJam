using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace GameMain.Scripts.UI
{
    public class MenuVideo : MonoBehaviour
    {
        [SerializeField] private RawImage RawImage;
        [SerializeField] private GameObject LevelMenu;
        private VideoPlayer m_VideoPlayer;

        private void Awake()
        {
            if (GameEntry.Setting.GetBool("VideoPlayed") == true)
            {
                OpenSelectPanel();
                return;
            }
            m_VideoPlayer = GetComponent<VideoPlayer>();
            m_VideoPlayer.loopPointReached += OnPlayEnd;
        }

        private void OnPlayEnd(VideoPlayer player)
        {
            GameEntry.Setting.SetBool("VideoPlayed",true);
            var texture = RawImage.texture;
            (texture as RenderTexture).Release();
            OpenSelectPanel();
        }

        private void OpenSelectPanel()
        {
            RawImage.gameObject.SetActive(false);
            gameObject.SetActive(false);
            LevelMenu.gameObject.SetActive(true);
        }

        public void Replay()
        {
            LevelMenu.gameObject.SetActive(false);
            RawImage.gameObject.SetActive(true);
            gameObject.SetActive(true);
            m_VideoPlayer.Play();
        }
    }
}