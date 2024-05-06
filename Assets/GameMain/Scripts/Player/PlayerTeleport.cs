using System;
using UnityEngine;

namespace GameMain
{
    public class PlayerTeleport : MonoBehaviour
    {
        private Animator m_Animator;
        private void Awake()
        {
            m_Animator = GetComponent<Animator>();
            m_Animator.Play("TeleportAnim");
        }
        public void OnPlayEnd()
        {
            Destroy(gameObject);
        }
    }
}