using System.Collections.Generic;
using Cinemachine;
using GameFramework.DataTable;
using GameFramework.Event;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public enum GameMode
    {
        Level,
        Endless
    }

    public  class GameBase
    {
        public GameMode GameMode { get; }

        public bool Inited => m_Inited;

        protected bool m_Inited;
        protected Player m_Player;
        protected CinemachineVirtualCamera vcam1;

        public virtual void Init()
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
                drPlayer.MaxHp, drPlayer.Damage, drPlayer.MoveSpeed, weaponDatas);
            GameEntry.Entity.ShowPlayer(playerData);
            vcam1 = GameObject.FindObjectOfType<CinemachineVirtualCamera>();
        }

        public virtual void Update()
        {
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
                vcam1.Follow = m_Player.transform;
                m_Inited = true;
            }
        }
    }
}