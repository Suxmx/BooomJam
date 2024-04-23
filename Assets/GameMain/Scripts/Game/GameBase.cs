using System.Collections.Generic;
using Cinemachine;
using GameFramework.DataTable;
using GameFramework.Event;
using GameFramework.Resource;
using MyTimer;
using Pathfinding;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public enum GameMode
    {
        Level,
        Endless
    }


    public class GameBase
    {
        public static GameBase Instance;

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
        public int Level { get; }

        public bool Inited => m_Inited;

        protected Dictionary<string, bool> m_InitDict;
        protected bool m_Inited;
        protected Player m_Player;
        protected List<GameScene> m_GameScenes;
        protected List<NavGraph> m_Graphs;
        protected int m_CurrentGameSceneIndex;
        protected GameScene m_CurrentGameScene => m_GameScenes[m_CurrentGameSceneIndex];
        protected CinemachineVirtualCamera vcam1;
        protected AstarPath m_AstarPath;


        public virtual void Init()
        {
            m_InitDict = new();
            m_GameScenes = new() { null, null };
            m_Graphs = new() { null, null };
            LoadGameScene();
            LoadPlayer();
        }

        #region Initialize

        protected void CheckInitState()
        {
            if (m_Inited) return;
            foreach (var pair in m_InitDict)
            {
                if (!pair.Value)
                {
                    Log.Info(pair.Key);
                    return;
                }
            }

            m_Inited = true;
        }

        private void LoadGameScene()
        {
            //读取关卡场景
            IDataTable<DRLevel> dtLevel = GameEntry.DataTable.GetDataTable<DRLevel>();
            DRLevel drLevel = dtLevel.GetDataRow(Level);
            string gameScene1 = AssetUtility.GetGameSceneAsset(drLevel.GameScene1);
            string gameScene2 = AssetUtility.GetGameSceneAsset(drLevel.GameScene2);
            m_InitDict.Add(gameScene1, false);
            m_InitDict.Add(gameScene2, false);
            GameEntry.Resource.LoadAsset(AssetUtility.GetEntityAsset("PathGraphScanner"), typeof(GameObject), 999, new LoadAssetCallbacks(
                (assetName, asset, duration, userData) =>
                {
                    GameObject obj = (GameObject)asset;
                    obj = GameEntry.InstantiateHelper(obj);
                    m_AstarPath = obj.GetComponent<AstarPath>();
                },
                (assetName, asset, duration, userData) => { Log.Error($"Load AstarPath Failure!"); }
            ));
            GameEntry.Resource.LoadAsset(gameScene1, typeof(GameObject), 100, new LoadAssetCallbacks
            ((assetName, asset, duration, userData) =>
                {
                    GameObject obj = (GameObject)asset;
                    m_InitDict[gameScene1] = true;
                    obj = GameEntry.InstantiateHelper(obj);
                    m_GameScenes[0] = obj.GetComponent<GameScene>();
                    m_AstarPath.Scan(m_AstarPath.graphs[0]);
                    m_Graphs[0] = m_AstarPath.graphs[0];
                },
                (assetName, asset, duration, userData) => { Log.Error($"Load GameScene{assetName} Failure!"); }
            ));
            GameEntry.Resource.LoadAsset(gameScene2, typeof(GameObject), 90, new LoadAssetCallbacks
            ((assetName, asset, duration, userData) =>
                {
                    GameObject obj = (GameObject)asset;
                    m_InitDict[gameScene2] = true;
                    obj = GameEntry.InstantiateHelper(obj);
                    m_GameScenes[1] = obj.GetComponent<GameScene>();
                    //预扫描NavGraph并存储
                    m_GameScenes[0].gameObject.SetActive(false);
                    m_AstarPath.Scan(m_AstarPath.graphs[1]);
                    m_GameScenes[0].gameObject.SetActive(true);
                    m_Graphs[1] = m_AstarPath.graphs[1];
                    ((GameObject)asset).SetActive(false);
                    m_AstarPath.data.graphs = new[] { m_Graphs[0] };
                },
                (assetName, asset, duration, userData) => { Log.Error($"Load GameScene{assetName} Failure!"); }
            ));
        }

        private void LoadPlayer()
        {
            GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, OnShowPlayerSuccess);
            //读取人物数据
            IDataTable<DRPlayer> dtPlayer = GameEntry.DataTable.GetDataTable<DRPlayer>();
            DRPlayer drPlayer = dtPlayer.GetDataRow((int)EPlayerType.Default);
            //读取武器数据
            List<WeaponData> weaponDatas = new();
            foreach (var id in drPlayer.WeaponDataIds)
            {
                IDataTable<DRWeapon> dtWeapon = GameEntry.DataTable.GetDataTable<DRWeapon>();
                DRWeapon drWeapon = dtWeapon.GetDataRow(id);
                weaponDatas.Add(WeaponFactory.GetWeaponData(drWeapon));
            }

            //打包数据生成人物
            PlayerData playerData = new PlayerData(GameEntry.Entity.GenerateSerialId(), (int)EPlayerType.Default,
                drPlayer.MaxHp, drPlayer.Damage, drPlayer.MoveSpeed, drPlayer.ChangeSceneInterval, weaponDatas);
            GameEntry.Entity.ShowPlayer(playerData);

            m_InitDict.Add("Player", false);
            vcam1 = Object.FindObjectOfType<CinemachineVirtualCamera>();
        }

        #endregion

        public virtual void Update()
        {
            CheckInitState();
            if (!m_Inited) return;
            ChangeScene();
            
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
            }
        }
        

        public virtual void OnGameEnd()
        {
            GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, OnShowPlayerSuccess);
        }

        protected void OnShowPlayerSuccess(object sender, GameEventArgs e)
        {
            ShowEntitySuccessEventArgs ne = (ShowEntitySuccessEventArgs)e;
            if (ne.EntityLogicType == typeof(Player))
            {
                m_Player = (Player)ne.Entity.Logic;
                // vcam1.Follow = m_Player.transform;
                m_InitDict["Player"] = true;
            }
        }

        public Player GetPlayer()
        {
            return m_Player;
        }
    }
}