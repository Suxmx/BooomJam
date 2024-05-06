using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace GameMain.Scripts.UI
{
    public class MenuVideo : MonoBehaviour
    {
        [SerializeField] private RawImage RawImage;
        private VideoPlayer m_VideoPlayer;

        private void Awake()
        {
            m_VideoPlayer = GetComponent<VideoPlayer>();
            m_VideoPlayer.loopPointReached += OnPlayEnd;
        }

        private void OnPlayEnd(VideoPlayer player)
        {
            var texture = RawImage.texture;
            (texture as RenderTexture).Release();
            RawImage.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }

        public void Replay()
        {
            RawImage.gameObject.SetActive(true);
            gameObject.SetActive(true);
            m_VideoPlayer.Play();
        }
    }
}