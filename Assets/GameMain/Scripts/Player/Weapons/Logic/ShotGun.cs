using GameFramework.Event;
using GameFramework.ObjectPool;
using GameFramework.Resource;
using GameMain.Scripts.Player.Weapons.ObjectPool;
using GameMain.Weapons.ObjectPool;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class ShotGun : WeaponBase, IHasObjectPool
    {
        protected int m_MinBulletNumPerFire;
        protected int m_MaxBulletNumPerFire;
        protected float m_BulletIntervalAngle;
        protected float m_MaxScaleFactor;
        protected float m_MinRecoilValue;
        protected float m_MaxRecoilValue;
        protected float m_Jump;
        protected ObjectPool<MyObjectBase, Bullet> m_BulletPool;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            ShotGunData data = (ShotGunData)userData;
            Damage = data.Damage;
            m_FireInterval = data.FireInterval;
            m_MinBulletNumPerFire = data.MinBulletNumPerFire;
            m_MaxBulletNumPerFire = data.MaxBulletNumPerFire;
            m_BulletIntervalAngle = data.BulletIntervalAngle;
            m_BulletRandomAngle = data.BulletRandomAngle;
            m_BulletSpeed = data.BulletSpeed;
            m_MaxChargeTime = data.MaxChargeTime;
            m_MaxScaleFactor = data.ChargeScaleFactor;
            m_MinRecoilValue = data.MinRecoilValue;
            m_MaxRecoilValue = data.MaxRecoilValue;
            m_Jump = data.Jump;
            
            GameEntry.Resource.LoadAsset(AssetUtility.GetEntityAsset("Bullet"), typeof(GameObject), 100,
                new LoadAssetCallbacks(
                    (assetName, asset, duration, userData) => { m_BulletTemplate = (GameObject)asset; },
                    (assetName, asset, duration, userData) => { Log.Error("加载Bullet预制体失败!"); }
                )
            );

            m_Muzzle = transform.Find("Muzzle");
            m_BulletPool = new ObjectPool<MyObjectBase, Bullet>(240, "ShotgunBulletPool", this);
            var hudTrans = transform.Find("HUDCanvas");
            if(hudTrans)
            {
                m_ChargeHUD = hudTrans.GetComponentInChildren<ShotGunHUD>();
                float minAngle = (m_MinBulletNumPerFire - 1) * m_BulletIntervalAngle;
                float maxAngle = (m_MaxBulletNumPerFire - 1) * m_BulletIntervalAngle;
                ((ShotGunHUD)m_ChargeHUD).Init(minAngle,maxAngle,m_Muzzle.position);
            }
        }

        protected override void Update()
        {
            base.Update();
            m_FireTimer += Time.deltaTime;
        }

        public override void Fire(Player player, float chargeTime)
        {
            m_ChargeHUD.Hide();
            //重置开火间隔
            if (m_FireTimer < m_FireInterval) return;
            m_FireTimer = 0;
            //弹道随机偏移
            float randomFireAngle = Random.Range(-m_BulletRandomAngle / 2, m_BulletRandomAngle / 2);
            Vector2 fireDirection = Quaternion.AngleAxis(randomFireAngle, Vector3.forward) * m_FireDirection;
            //计算蓄力影响
            int bulletNum;
            float scaleFactor;
            float RecoilValue;
            if (chargeTime >= m_MaxChargeTime)
            {
                bulletNum = m_MaxBulletNumPerFire;
                scaleFactor = m_MaxScaleFactor;
                RecoilValue = m_MaxRecoilValue;
            }
            else
            {
                bulletNum = (int)(m_MinBulletNumPerFire +
                                  (m_MaxBulletNumPerFire - m_MinBulletNumPerFire) * GetChargePercent());
                scaleFactor = 1 + (m_MaxScaleFactor - 1) * GetChargePercent();
                RecoilValue = (m_MinRecoilValue + 
                                  (m_MaxRecoilValue - m_MinRecoilValue ) * GetChargePercent());
            }

            //生成霰弹
            for (int i = 1; i <= bulletNum; i++)
            {
                BulletData data = new BulletData(GameEntry.Entity.GenerateSerialId(), (int)EWeapon.Bullet, false,
                    Damage, m_BulletSpeed, 5, scaleFactor);
                data.Position = m_Muzzle.position;
                Vector2 d;
                if (bulletNum % 2 == 0)
                {
                    int mid = (bulletNum) / 2 + 1;
                    d = Quaternion.AngleAxis(m_BulletIntervalAngle / 2 + (m_BulletIntervalAngle * (i - mid)),
                        Vector3.forward) * fireDirection;
                }
                else
                {
                    int mid = (bulletNum + 1) / 2;
                    d = Quaternion.AngleAxis(m_BulletIntervalAngle * (i - mid), Vector3.forward) *
                        fireDirection;
                }

                data.Direction = Quaternion.AngleAxis(0, Vector3.forward) * d;
                m_BulletPool.Spawn(data);
            }

            var recoilData = new RecoilData(m_FireDirection, RecoilValue, m_Jump);
            StartCoroutine(player.Recoil(recoilData));
        }

        public GameObject CreateObject()
        {
            return Instantiate(m_BulletTemplate);
        }
    }
}