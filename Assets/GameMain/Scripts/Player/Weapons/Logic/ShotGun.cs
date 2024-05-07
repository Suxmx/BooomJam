using Cinemachine;
using GameFramework.Event;
using GameFramework.Extensions.Sound;
using GameFramework.ObjectPool;
using GameFramework.Resource;
using GameMain.Scripts.Player.Weapons.ObjectPool;
using GameMain.Weapons.ObjectPool;
using MyTimer;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class ShotGun : WeaponBase, IHasObjectPool
    {
        protected float[] m_BulletChargePercentList;
        protected int[] m_BulletNumPerFireList;
        protected float m_BulletIntervalAngle;
        protected float m_MaxScaleFactor;
        protected float m_MinRecoilValue;
        protected float m_MaxRecoilValue;
        protected float m_Jump;
        private Animator m_Animator;
        private CinemachineImpulseSource m_Source;
        protected ObjectPool<MyObjectBase, Bullet> m_BulletPool;

        public override void Init(object userData)
        {
            ShotGunData data = (ShotGunData)userData;
            Damage = data.Damage;
            m_FireCountdownTimer = new CountdownTimer();
            m_FireCountdownTimer.Initialize(data.FireInterval);
            m_BulletNumPerFireList = new int[4];
            m_BulletChargePercentList = new float[4];
            for (int i = 0; i < 4; i++)
            {
                m_BulletNumPerFireList[i] = data.BulletNumPerFireList[i];
                m_BulletChargePercentList[i] = data.BulletChargePercentList[i];
            }
            m_BulletIntervalAngle = data.BulletIntervalAngle;
            m_BulletRandomAngle = data.BulletRandomAngle;
            m_BulletSpeed = data.BulletSpeed;
            m_MaxChargeTime = data.MaxChargeTime;
            m_MaxScaleFactor = data.ChargeScaleFactor;
            m_MinRecoilValue = data.MinRecoilValue;
            m_MaxRecoilValue = data.MaxRecoilValue;
            m_Jump = data.Jump;
            
            m_BulletTemplate = data.BulletPrefab;
            m_Muzzle = transform.Find("Muzzle");
            m_Animator = GetComponent<Animator>();
            m_BulletPool = new ObjectPool<MyObjectBase, Bullet>(240, "ShotgunBulletPool", this);
            var hudTrans = transform.Find("HUDCanvas");
            if(hudTrans)
            {
                m_ChargeHUD = hudTrans.GetComponentInChildren<ShotGunHUD>();
                float minAngle = (m_BulletNumPerFireList[0] - 1) * m_BulletIntervalAngle;
                float maxAngle = (m_BulletNumPerFireList[3] - 1) * m_BulletIntervalAngle;
                ((ShotGunHUD)m_ChargeHUD).Init(minAngle,maxAngle,m_BulletChargePercentList,m_Muzzle.position);
            }
            m_Source = GetComponent<CinemachineImpulseSource>();
        }
        

        public override void Fire(Player player, float chargeTime)
        {
            m_ChargeHUD.Hide();
            //重置开火间隔
            if (!m_FireCountdownTimer.Completed) return;
            m_FireCountdownTimer.Restart();
            m_Animator.Play("ShotGunFire");
            //弹道随机偏移
            float randomFireAngle = Random.Range(-m_BulletRandomAngle / 2, m_BulletRandomAngle / 2);
            Vector2 fireDirection = Quaternion.AngleAxis(randomFireAngle, Vector3.forward) * m_FireDirection;
            //计算蓄力影响
            int bulletNum=0;
            float scaleFactor;
            float RecoilValue;
            if (chargeTime >= m_MaxChargeTime)
            {
                bulletNum = m_BulletNumPerFireList[3];
                scaleFactor = m_MaxScaleFactor;
                RecoilValue = m_MaxRecoilValue;
            }
            else
            {
                for (int i = 3; i >= 0; i--)
                {
                    if (GetChargePercent() >= m_BulletChargePercentList[i])
                    {
                        bulletNum = m_BulletNumPerFireList[i];
                        break;
                    }
                }
                scaleFactor = 1 + (m_MaxScaleFactor - 1) * GetChargePercent();
                RecoilValue = (m_MinRecoilValue + 
                                  (m_MaxRecoilValue - m_MinRecoilValue ) * GetChargePercent());
            }

            //生成霰弹
            for (int i = 1; i <= bulletNum; i++)
            {
                BulletData data = new BulletData(GameEntry.Entity.GenerateSerialId(), (int)EWeapon.Bullet, false,
                    Damage, m_BulletSpeed, 0.2f, scaleFactor);
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
            player.CauseRecoil(recoilData,m_Source);
            if (Random.Range(0, 1f) < 0.5f)
            {
                GameEntry.Sound.PlaySoundM("shotgun1","mp3");
            }
            else
            {
                GameEntry.Sound.PlaySoundM("shotgun2");
            }
        }

        public GameObject CreateObject()
        {
            return Instantiate(m_BulletTemplate);
        }
    }
}