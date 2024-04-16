using System.Collections.Generic;
using UnityEngine;

namespace GameMain
{
    public class GameScene : MonoBehaviour
    {
        protected List<GameObject> m_Enemies;
        public void OnChangeSceneToAnother()
        {
            gameObject.SetActive(false);            
        }

        public void OnChangeSceneToThis()
        {
            gameObject.SetActive(true);
        }
    }
}