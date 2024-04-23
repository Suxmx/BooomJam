using System;
using GameMain.Scripts.Player.Weapons.ObjectPool;
using MyTimer;
using UnityEngine;

namespace GameMain
{
    public class EnemySpawner : MonoBehaviour,IHasObjectPool
    {
        private ObjectPool<MyObjectBase, Enemy> m_EnemyPool;
        private TimerOnly m_Timer;
        // private 

        private void Awake()
        {
            m_EnemyPool = new ObjectPool<MyObjectBase, Enemy>(240, "EnemyPool", this);
        }

        public void SpawnTestEnemy()
        {
            
        }

        public GameObject CreateObject()
        {
            return new GameObject();
            // return Instantiate()
        }
    }
}