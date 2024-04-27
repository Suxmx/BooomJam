using System;
using GameFramework.Resource;
using GameMain.Scripts.Player.Weapons.ObjectPool;
using MyTimer;
using UnityEngine;
using UnityGameFramework.Runtime;
using Random = UnityEngine.Random;

namespace GameMain
{
    public class Bow : WeaponBase, IHasObjectPool
    {
        protected float m_MinArrowAliveTime;
        protected float m_MaxArrowAliveTime;
        protected float m_MaxRandomAngle;
        protected float m_MinRandomAngle;
        protected ObjectPool<MyObjectBase, Arrow> m_ArrowPool;

        public override void Init(object userData)
        {
            BowData data = (BowData)userData;
            Damage = data.Damage;
            m_FireCountdownTimer = new CountdownTimer();
            m_FireCountdownTimer.Initialize(data.FireInterval);
            m_MaxChargeTime = data.MaxChargeTime;
            m_MinArrowAliveTime = data.MinArrowAliveTime;
            m_MaxArrowAliveTime = data.MaxArrowAliveTime;
            m_MinRandomAngle = data.MinRandomAngle;
            m_MaxRandomAngle = data.MaxRandomAngle;
            m_BulletSpeed = data.BulletSpeed;

            m_Muzzle = transform.Find("Muzzle");
            m_ArrowPool = new ObjectPool<MyObjectBase, Arrow>(240, "BowArrowPool", this);
            // GameEntry.Resource.LoadAsset(AssetUtility.GetPrefabAsset("Bullet"), typeof(GameObject), 100,
            //     new LoadAssetCallbacks(
            //         (assetName, asset, duration, userData) => { m_BulletTemplate = (GameObject)asset; },
            //         (assetName, asset, duration, userData) => { Log.Error("加载Arrow预制体失败!"); }
            //     )
            // );
            m_BulletTemplate = data.BulletPrefab;
            var hudTrans = transform.Find("HUDCanvas");
            if (hudTrans)
            {
                m_ChargeHUD = hudTrans.GetComponentInChildren<BowHUD>();
                float minLength = m_BulletSpeed * m_MinArrowAliveTime;
                float maxLength = m_BulletSpeed * m_MaxArrowAliveTime;
                ((BowHUD)m_ChargeHUD).Init(minLength, maxLength,m_MinRandomAngle,m_MaxRandomAngle, m_Muzzle.position);
                m_ChargeHUD.Hide();
            }
        }

        protected override void Update()
        {
            base.Update();
            Debug.DrawRay(m_Muzzle.position,
                transform.right * (m_BulletSpeed * (m_MinArrowAliveTime +
                                                    (m_MaxArrowAliveTime - m_MinArrowAliveTime) * GetChargePercent())),
                Color.cyan);
        }

        public override void Charge(float deltaTime)
        {
            base.Charge(deltaTime);
        }

        public override void Fire(Player player, float chargeTime)
        {
            m_ChargeHUD.Hide();
            //重置开火间隔
            if (!m_FireCountdownTimer.Completed) return;
            m_FireCountdownTimer.Restart();
            //计算蓄力影响
            float aliveTime = m_MinArrowAliveTime +
                              (m_MaxArrowAliveTime - m_MinArrowAliveTime) * GetChargePercent();
            float randomFireAngle = m_MaxRandomAngle - (m_MaxRandomAngle - m_MinRandomAngle) * GetChargePercent();
            randomFireAngle = Random.Range(-randomFireAngle / 2, randomFireAngle / 2);
            //弹道随机偏移
            Vector2 fireDirection = Quaternion.AngleAxis(randomFireAngle, Vector3.forward) * m_FireDirection;
            BulletData data = new BulletData(GameEntry.Entity.GenerateSerialId(), (int)EWeapon.Arrow, true, Damage,
                m_BulletSpeed, aliveTime, fireDirection);
            data.Position = m_Muzzle.position;
            data.Player = player;
            m_ArrowPool.Spawn(data);
        }

        public GameObject CreateObject()
        {
            return Instantiate(m_BulletTemplate);
        }
    }
}