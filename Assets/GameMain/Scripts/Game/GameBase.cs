using System.Collections.Generic;
using Cinemachine;
using GameFramework.DataTable;
using GameFramework.Event;
using GameFramework.Resource;
using GameMain.Scripts.UI;
using MyTimer;
using Pathfinding;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
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

        public bool Pause { get; protected set; }

        public GameBase(GameMode gameMode)
        {
            GameMode = gameMode;
        }

        public GameBase(GameMode gameMode, int level)
        {
            GameMode = gameMode;
            Level = level + 1;
        }

        public GameMode GameMode { get; }
        public int Level { get; set; }
        protected int m_Combo;

        protected int Combo
        {
            get => m_Combo;
            set
            {
                m_Combo = value;
                m_ComboText.text = $"{value.ToString()}\n<size=30>combo</size>";
            }
        }

        public int Score
        {
            get { return m_Score; }
            protected set
            {
                m_Score = value;
                m_ScoreText.text = $"Score:{value.ToString()}";
            }
        }

        protected int m_Score = 0;
        protected int m_ScoreFactor = 1;

        protected bool m_Inited;
        protected Player m_Player;
        protected PublicObjectPool m_PublicObjectPool;
        protected List<GameScene> m_GameScenes;
        protected List<NavGraph> m_Graphs;
        protected int m_CurrentGameSceneIndex;
        protected GameScene m_CurrentGameScene => m_GameScenes[m_CurrentGameSceneIndex];
        protected AstarPath m_AstarPath;
        protected PauseUI m_PauseUI;
        protected SettleUI m_SettleUI;
        protected TextMeshProUGUI m_ScoreText;
        protected TextMeshProUGUI m_ComboText;
        protected EnemySpawner m_Spawner;
        protected List<FMagicCircleData> m_MagicCircleDatas = new();
        protected float m_Timer = 0f;
        protected float m_LastEnemyDie = 0f;
        public bool NoMagicCircleTriggered;
        public bool NoFireDamage;


        #region Initialize

        public virtual void Init(GameBaseData data)
        {
            GameObject.Find($"Tip{Level}").GetComponent<TipsUI>().Show();
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
            Transform canvas = GameObject.Find("Canvas").transform;
            m_PauseUI = canvas.Find("PausePage").GetComponent<PauseUI>();
            canvas.Find("Pause").GetComponent<Button>().onClick.AddListener(PauseGame);
            m_Spawner = GameObject.Find("Spawner").GetComponent<EnemySpawner>();
            //初始化魔法阵
            foreach (var scene in m_GameScenes)
            {
                foreach (var d in scene.MagicCircleDatas)
                {
                    d.circle.transform.parent = null;
                    m_MagicCircleDatas.Add(d);
                }
            }

            //找到分数UI
            m_ScoreText = GameObject.Find("ScoreText").GetComponentInChildren<TextMeshProUGUI>();
            m_SettleUI = canvas.Find("StarPanel").GetComponent<SettleUI>();
            m_ComboText = GameObject.Find("ComboText").GetComponent<TextMeshProUGUI>();
            NoMagicCircleTriggered = true;
            NoFireDamage = true;
        }

        #endregion

        public EnemySpawner GetSpawner()
        {
            return m_Spawner;
        }

        public void PauseGame()
        {
            Time.timeScale = 0f;
            m_PauseUI.gameObject.SetActive(true);
            Pause = true;
        }

        public void ContinueGame()
        {
            Time.timeScale = 1f;
            m_PauseUI.gameObject.SetActive(false);
            Pause = false;
        }

        public virtual void Update(float dT)
        {
            m_Timer += dT;
            float comboInterval = m_Timer - m_LastEnemyDie;
            if (comboInterval >= 3f) Combo = 1;
            foreach (var data in m_MagicCircleDatas)
            {
                if (m_Timer > data.ShowTime && !data.hasShowed)
                {
                    data.circle.gameObject.SetActive(true);
                    data.hasShowed = true;
                }
            }

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

        public void OnEnemyDie()
        {
            float interval = m_Timer - m_LastEnemyDie;
            m_LastEnemyDie = m_Timer;
            Combo = interval <= 3f ? Combo + 1 : 1;
            Score += Combo  * 10;
        }

        public static Dictionary<int, int> scoreDict = new() { { 1, 1000 }, { 2, 1200 }, { 3, 1500 } };

        public void OnWin()
        {
            Time.timeScale = 0f;
            m_SettleUI.gameObject.SetActive(true);
            List<bool> stars = new();
            stars.Add(true);
            stars.Add(Score >= scoreDict[Level]);
            switch (Level)
            {
                case 1:
                    stars.Add(m_Player.GetHp() >= 5);
                    break;
                case 2:
                    stars.Add(NoMagicCircleTriggered);
                    break;
                case 3:
                    stars.Add(NoFireDamage);
                    break;
            }

            m_SettleUI.SetStar(stars);
        }
    }
}