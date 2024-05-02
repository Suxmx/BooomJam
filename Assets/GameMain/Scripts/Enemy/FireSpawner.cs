using System;
using GameMain.Scripts.Player.Weapons.ObjectPool;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class FireSpawner : MonoBehaviour, IHasObjectPool
    {
        [SerializeField] private GameObject m_FireTemplate;
        private ObjectPool<MyObjectBase, Enemy> m_FirePool;
        
        private void Awake()
        {
            m_FirePool = new ObjectPool<MyObjectBase, Enemy>(100, "FirePool", this);
        }

        public void SpawnFire()
        {   
            var e = m_FirePool.Spawn();
            e.transform.position = transform.position;
        } 
        

        public GameObject CreateObject()
        {
            return Instantiate(m_FireTemplate);
        }
        
    }
}
