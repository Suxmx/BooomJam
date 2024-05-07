using System;
using System.Collections;
using System.Collections.Generic;
using GameMain.Scripts.Player.Weapons.ObjectPool;
using MyTimer;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityGameFramework.Runtime;
using Random = UnityEngine.Random;

namespace GameMain
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField, LabelText("生成间隔")] private float m_SpawnInterval = 1f;
        // [SerializeField] private GameObject m_EnemyTemplate;

        [SerializeField] private GameObject m_BoneTemplate;
        [SerializeField] private GameObject m_BluePumpkinTemplate;
        [SerializeField] private GameObject m_RedPumpkinTemplate;
        [SerializeField] private GameObject m_GhostTemplate;
        private RepeatTimer m_SpawnTimer;
        private ObjectPool<MyObjectBase, Enemy> m_EnemyPool;

        private Bounds m_SpawnBounds;
        private Dictionary<string, Queue<GameObject>> m_Pools = new();
        private Dictionary<string, GameObject> m_TemplateDict = new();
        private Dictionary<string, Transform> m_PoolParentDict = new();

        private void OnDestroy()
        {
            m_SpawnTimer.Paused = true;
            StopAllCoroutines();
        }

        private void Awake()
        {
            m_SpawnBounds = GameObject.Find("EnemySpawnBound").GetComponent<BoxCollider2D>().bounds;
            m_SpawnTimer = new RepeatTimer();
            m_SpawnTimer.Initialize(m_SpawnInterval);
            m_SpawnTimer.OnComplete += SpawnTestEnemy;
            m_TemplateDict.Add("Bone", m_BoneTemplate);
            m_TemplateDict.Add("BluePumpkin", m_BluePumpkinTemplate);
            m_TemplateDict.Add("RedPumpkin", m_RedPumpkinTemplate);
            m_TemplateDict.Add("Ghost", m_GhostTemplate);
            foreach (var key in m_TemplateDict.Keys)
            {
                m_Pools.Add(key, new Queue<GameObject>());
                GameObject parent = new GameObject(key);
                m_PoolParentDict.Add(key, parent.transform);
                parent.transform.SetParent(transform);
            }
        }

        public void SpawnTestEnemy()
        {
            // var e=m_EnemyPool.Spawn();
            // e.transform.position = RandomPosition();
            //
            int rand = Random.Range(1, GameBase.Instance.Level >= 3 ? 4 : 3);
            Enemy e;
            switch (rand)
            {
                case 1:
                    e = Spawn("Bone");
                    e.transform.position = RandomPosition();
                    break;
                case 2:
                    e = Spawn("Ghost");
                    e.transform.position = RandomPosition();
                    ((Ghost)e).Init(Random.Range(0, 2), GameBase.Instance.GetGameSceneIndex());
                    break;
                case 3:
                    int pumpkinRand = Random.Range(0, 2);
                    e = pumpkinRand == 0 ? Spawn("BluePumpkin") : Spawn("RedPumpkin");
                    e.transform.position = RandomPosition();
                    ((Pumpkin)e).Init(pumpkinRand, GameBase.Instance.GetGameSceneIndex());
                    break;
            }
        }

        public void SpawnPlentyEnemy(int num)
        {
            StartCoroutine(ISpawnPlentyEnemy(num));
        }

        private IEnumerator ISpawnPlentyEnemy(int num)
        {
            for (int i = 1; i <= num; i++)
            {
                SpawnTestEnemy();
                yield return new WaitForSeconds(0.3f);
            }
        }

        private Enemy Spawn(string enemyName)
        {
            if (!m_Pools.ContainsKey(enemyName))
            {
                Log.Error($"不存在{enemyName}对应的对象池");
                return null;
            }

            if (m_Pools[enemyName].Count > 0)
            {
                var e = m_Pools[enemyName].Dequeue().GetComponent<Enemy>();
                e.gameObject.SetActive(true);
                e.OnShow(null);
                return e;
            }
            else
            {
                var e = Instantiate(m_TemplateDict[enemyName], m_PoolParentDict[enemyName]).GetComponent<Enemy>();
                e.OnInit(this);
                e.SetName(enemyName);
                e.OnShow(null);
                return e;
            }
        }

        public void Unspawn(Enemy enemy)
        {
            enemy.gameObject.SetActive(false);
            m_Pools[enemy.GetName()].Enqueue(enemy.gameObject);
        }


        private Vector2 RandomPosition()
        {
            float x = Random.Range(m_SpawnBounds.min.x, m_SpawnBounds.max.x);
            float y = Random.Range(m_SpawnBounds.min.y, m_SpawnBounds.max.y);
            var pos = new Vector2(x, y);
            if (Vector2.Distance(pos, GameBase.Instance.GetPlayer().transform.position) < 1f)
                return RandomPosition();
            return pos;
        }
    }
}