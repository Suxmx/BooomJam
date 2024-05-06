using System;
using UnityEngine;

namespace GameMain
{
    public class PlayerDieAnim : MonoBehaviour
    {
        private void Awake()
        {
            GetComponent<Animator>().Play("Die");
        }

        public void OnDieAnimEnd()
        {
            GameBase.Instance.ReturnMenu();
        }
    }
}