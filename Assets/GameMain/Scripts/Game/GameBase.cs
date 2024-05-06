using System.Collections.Generic;
using Cinemachine;
using GameFramework.DataTable;
using GameFramework.Event;
using GameFramework.Resource;
using GameMain.Scripts.UI;
using MyTimer;
using Pathfinding;
using UnityEngine;
using UnityEngine.Events;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public enum GameMode
    {
        Level,
        Endless
    }

    public class GameBaseData
    {
        public Dictionary<string, GameObject> Prefabs = new();
        public PlayerData PlayerData;
    }


    public class GameBase
    {
        public static GameBase Instance;
        public UnityAction<int> OnChangeGameScene;

        public GameBase(GameMode gameMode)
        {
            GameMode = gameMode;
        }

        public GameBase(GameMode gameMode, int level)
        {
            GameMode = gameMode;
            Level = level;
        }

        public GameMode GameMode { get; }
        public int Level { get; set; }

        protected bool m_Inited;
        protected Player m_Player;
        protected PublicObjectPool m_PublicObjectPool;
        protected List<GameScene> m_GameScenes;
        protected List<NavGraph> m_Graphs;
        protected int m_CurrentGameSceneIndex;
        protected GameScene m_CurrentGameScene => m_GameScenes[m_CurrentGameSceneIndex];
        protected AstarPath m_AstarPath;


        #region Initialize

        public virtual void Init(GameBaseData data)
        {
            GameObject.Find($"Tip{Level + 1}").GetComponent<TipsUI>().Show();
            //生成初始化Player
            var player = Object.Instantiate(data.Prefabs["Player"]).GetComponent<Player>();
            player.Init(data.PlayerData);
            m_Player = player.GetComponent<Player>();
            //生成PathGraphScanner
            var pathScanner = Object.Instantiate(data.Prefabs["PathGraphScanner"]);
            m_AstarPath = pathScanner.GetComponent<AstarPath>();
            //生成初始化GameScene
            m_Graphs = new() { null, null };
            m_GameScenes = new() { null, null };
            var scene1 = Object.Instantiate(data.Prefabs["GameScene1"]);
            m_GameScenes[0] = scene1.GetComponent<GameScene>();
            m_AstarPath.Scan(m_AstarPath.graphs[0]);
            m_Graphs[0] = m_AstarPath.graphs[0];
            var scene2 = Object.Instantiate(data.Prefabs["GameScene2"]);
            m_GameScenes[1] = scene2.GetComponent<GameScene>();
            m_GameScenes[0].gameObject.SetActive(false);
            m_AstarPath.Scan(m_AstarPath.graphs[1]);
            m_GameScenes[0].gameObject.SetActive(true);
            m_Graphs[1] = m_AstarPath.graphs[1];
            scene2.SetActive(false);
            m_AstarPath.data.graphs = new[] { m_Graphs[0] };
            //生成对象池
            m_PublicObjectPool = Object.Instantiate(data.Prefabs["PublicObjectPool"]).GetComponent<PublicObjectPool>();
            m_PublicObjectPool.RegisterTemplate("BulletExplode", data.Prefabs["BulletExplode"]);
            m_PublicObjectPool.RegisterTemplate("BluePumpkinFire", data.Prefabs["BluePumpkinFire"]);
            m_PublicObjectPool.RegisterTemplate("RedPumpkinFire", data.Prefabs["RedPumpkinFire"]);
            
        }

        #endregion

        public virtual void Update()
        {
            ChangeScene();
        }

        public int GetGameSceneIndex()
        {
            return m_CurrentGameSceneIndex;
        }

        protected void ChangeScene()
        {
            if (Input.GetMouseButtonDown(1))
            {
                if (!m_Player.CanChangeWeapon()) return;
                //TODO:检测障碍物
                m_Player.ChangeWeapon();
                m_CurrentGameScene.OnChangeSceneToAnother();
                m_CurrentGameSceneIndex = m_CurrentGameSceneIndex == 0 ? 1 : 0;
                //更换当前预烘焙的NavGraph
                m_AstarPath.data.graphs = new[] { m_Graphs[m_CurrentGameSceneIndex] };
                //更新PathFinder Gizmos
                AstarPath.active.hierarchicalGraph.RecalculateIfNecessary();
                m_CurrentGameScene.OnChangeSceneToThis();
                OnChangeGameScene?.Invoke(m_CurrentGameSceneIndex);
            }
        }

        public void ReturnMenu()
        {
            (GameEntry.Procedure.CurrentProcedure as ProcedureMainGame).ReturnToMenu();
        }


        public Player GetPlayer()
        {
            return m_Player;
        }

        public PublicObjectPool GetObjectPool()
        {
            return m_PublicObjectPool;
        }
    }
}