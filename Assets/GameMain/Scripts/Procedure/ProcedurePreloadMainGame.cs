using System.Collections.Generic;
using GameFramework;
using GameFramework.DataTable;
using GameFramework.Event;
using GameFramework.Extensions;
using GameFramework.Procedure;
using GameFramework.Resource;
using GameMain;
using UnityEngine;
using UnityGameFramework.Runtime;
using GameEntry = GameMain.GameEntry;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace GameMain
{
    public class ProcedurePreloadMainGame : ProcedureBase
    {
        private Dictionary<string, bool> m_LoadedAssetFlag = new();
        private GameBaseData m_LoadedData = new();

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            m_LoadedAssetFlag = new();
            m_LoadedData = new();
            LoadPrefab(AssetUtility.GetPrefabAsset("PublicObjectPool"),"PublicObjectPool");
            LoadPlayer();
            LoadGameScene(procedureOwner.GetData<VarInt32>("Level"));
            LoadPathGraphScanner();
            LoadPrefab(AssetUtility.GetPrefabAsset("BluePumpkinFire"),"BluePumpkinFire");
            LoadPrefab(AssetUtility.GetPrefabAsset("RedPumpkinFire"),"RedPumpkinFire");
            var pools=GameEntry.ObjectPool.GetAllObjectPools();
            foreach (var pool in pools)
            {
                GameEntry.ObjectPool.DestroyObjectPool(pool);
            }
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            foreach (KeyValuePair<string, bool> loadedFlag in m_LoadedAssetFlag)
            {
                if (!loadedFlag.Value)
                {
                    return;
                }
            }

            //继续打包PlayerData
            foreach (var data in m_LoadedData.PlayerData.WeaponsDatas)
            {
                data.WeaponPrefab = m_LoadedData.Prefabs[data.name];
                data.BulletPrefab = m_LoadedData.Prefabs[data.name + "_Bullet"];
                data.BulletExplodePrefab = m_LoadedData.Prefabs["BulletExplode"];
            }

            var tmp = new VarObject();
            tmp.Value = m_LoadedData;
            procedureOwner.SetData<VarObject>("GameData", tmp);
            ChangeState<ProcedureChangeScene>(procedureOwner);
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
            procedureOwner.SetData<VarInt32>("NextSceneId", GameEntry.Config.GetInt("Scene.Main"));
        }

        private void LoadPrefab(string path, string nameInDict)
        {
            m_LoadedAssetFlag.Add(nameInDict, false);
            GameEntry.Resource.LoadAsset(path, typeof(GameObject), 999,
                new LoadAssetCallbacks(
                    (assetName, asset, duration, userData) =>
                    {
                        Log.Info($"Load {nameInDict} Success");
                        m_LoadedData.Prefabs.Add(nameInDict, (GameObject)asset);
                        m_LoadedAssetFlag[nameInDict] = true;
                    },
                    (assetName, asset, duration, userData) => { Log.Error($"Load Prefab {path} Failure!"); }
                ));
        }

        private void LoadPrefabByPath(string path, string nameInDict = null)
        {
        }

        private void LoadPathGraphScanner()
        {
            LoadPrefab(AssetUtility.GetPrefabAsset("PathGraphScanner"), "PathGraphScanner");
        }

        private void LoadPlayer()   
        {
            //读取人物数据
            IDataTable<DRPlayer> dtPlayer = GameEntry.DataTable.GetDataTable<DRPlayer>();
            DRPlayer drPlayer = dtPlayer.GetDataRow((int)EPlayerType.Default);
            //读取武器数据
            List<WeaponData> weaponDatas = new();
            foreach (var name in drPlayer.WeaponNames)
            {
                IDataTable<DRWeapon> dtWeapon = GameEntry.DataTable.GetDataTable<DRWeapon>();
                DRWeapon drWeapon = dtWeapon.GetDataRow(DRWeapon.WeaponName2Id[name]);
                weaponDatas.Add(WeaponFactory.GetWeaponData(drWeapon));
                LoadPrefab(AssetUtility.GetPrefabAsset(name), name);
                LoadPrefab(AssetUtility.GetPrefabAsset(name+"_Bullet"), name+"_Bullet");
            }

            //打包数据生成人物
            PlayerData playerData = new PlayerData(
                drPlayer.MaxHp, drPlayer.Damage, drPlayer.MoveSpeed, drPlayer.ChangeSceneInterval, weaponDatas);
            m_LoadedData.PlayerData = playerData;
            LoadPrefab(AssetUtility.GetPrefabAsset("Player"), "Player");
            LoadPrefab(AssetUtility.GetPrefabAsset("BulletExplode"),"BulletExplode");
        }

        private void LoadGameScene(int level)
        {
            IDataTable<DRLevel> dtLevel = GameEntry.DataTable.GetDataTable<DRLevel>();
            DRLevel drLevel = dtLevel.GetDataRow(level);
            LoadPrefab(AssetUtility.GetGameSceneAsset(drLevel.GameScene1), "GameScene1");
            LoadPrefab(AssetUtility.GetGameSceneAsset(drLevel.GameScene2), "GameScene2");
        }
    }
}