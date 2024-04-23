using System;
using GameMain.Scripts.Player.Weapons.ObjectPool;
using MyTimer;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameMain
{
    public class EnemySpawner : MonoBehaviour, IHasObjectPool
    {
        [SerializeField, LabelText("生成间隔")] private float m_SpawnInterval=1f;
        [SerializeField] private GameObject m_EnemyTemplate;
        private ObjectPool<MyObjectBase, Enemy> m_EnemyPool;
        private RepeatTimer m_SpawnTimer;

        private Bounds m_SpawnBounds;
        // private 

        private void Awake()
        {
            m_EnemyPool = new ObjectPool<MyObjectBase, Enemy>(240, "EnemyPool", this);
            m_SpawnBounds = GameObject.Find("EnemySpawnBound").GetComponent<BoxCollider2D>().bounds;
            m_SpawnTimer = new RepeatTimer();
            m_SpawnTimer.Initialize(m_SpawnInterval);
            m_SpawnTimer.OnComplete += SpawnTestEnemy;
        }

        public void SpawnTestEnemy()
        {
            var e=m_EnemyPool.Spawn();
            e.transform.position = RandomPosition();
        }

        public GameObject CreateObject()
        {
            return Instantiate(m_EnemyTemplate);
        }

        private Vector2 RandomPosition()
        {
            float x = Random.Range(m_SpawnBounds.min.x, m_SpawnBounds.max.x);
            float y = Random.Range(m_SpawnBounds.min.y, m_SpawnBounds.max.y);
            var pos = new Vector2(x,y);
            if (Vector2.Distance(pos, GameBase.Instance.GetPlayer().transform.position) < 1f)
                return RandomPosition();
            return pos;
        }
    }
}