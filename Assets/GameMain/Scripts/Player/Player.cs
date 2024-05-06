using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using GameFramework.Event;
using MyTimer;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class Player : MonoBehaviour, IAttackable
    {
        private enum EWeapon
        {
            ShotGun,
            Bow
        }

//赶进度
        [SerializeField] private GameObject DieAnim;

        private SpriteRenderer m_SpriteRenderer;
        private Animator m_Animator;
        private Rigidbody2D m_Rigidbody;
        private Vector3 m_MoveDirection;
        private List<WeaponBase> m_Weapons;
        private WeaponBase m_CurrentWeapon;
        private PlayerStatusInfo m_PlayerStatusInfo;
        private CapsuleCollider2D m_Collider;
        private Image m_HpImage;
        private CountdownTimer m_InvincibleTimer;
        private CountdownTimer m_ChangeSceneTimer;
        private Coroutine m_IRecoil;
        private Coroutine m_IMoveTowards;
        private EWeapon m_CurrentWeaponState = EWeapon.ShotGun;
        public Lock m_InvincibleLock;
        private int m_CurrentWeaponIndex;
        private int m_ObstacleMask;
        public bool m_IsMovingToward = false;

        private bool Invincible
        {
            get { return !m_InvincibleLock.Unlocked; }
        }

        private int m_WeaponToLoad;
        private bool m_Inited;

        private float m_MoveSpeed => m_PlayerStatusInfo.MoveSpeed;

        public void Init(PlayerData data)
        {
            m_PlayerStatusInfo = new PlayerStatusInfo(data.MaxHp, data.MoveSpeed);
            m_WeaponToLoad = data.WeaponsDatas.Count;
            m_Weapons = new List<WeaponBase>();
            foreach (var weapon in data.WeaponsDatas)
            {
                var obj = Instantiate(weapon.WeaponPrefab, transform.Find("Weapons"), true);
                obj.transform.localPosition = new Vector3(0, -0.261999995f, 0);
                var weaponBase = obj.GetComponent<WeaponBase>();
                weaponBase.Init(weapon);
                m_Weapons.Add(weaponBase);
                if (m_CurrentWeapon == null)
                {
                    m_CurrentWeaponIndex = 0;
                    m_CurrentWeapon = weaponBase;
                }
                else obj.SetActive(false);
            }

            m_Rigidbody = GetComponent<Rigidbody2D>();
            m_SpriteRenderer = transform.Find("PlayerImage").GetComponent<SpriteRenderer>();
            m_Animator = transform.Find("PlayerImage").GetComponent<Animator>();

            m_InvincibleTimer = new CountdownTimer();
            m_ChangeSceneTimer = new CountdownTimer();
            m_ChangeSceneTimer.Initialize(data.ChangeSceneInterval);
            m_InvincibleTimer.Initialize(data.InvincibleTime, false);
            m_InvincibleLock = new();
            m_InvincibleLock.OnLock += () =>
            {
                transform.Find("PlayerImage").GetComponent<SpriteRenderer>().color = Color.yellow;
            };
            m_InvincibleLock.OnUnlock += () =>
            {
                transform.Find("PlayerImage").GetComponent<SpriteRenderer>().color = Color.white;
            };
            m_InvincibleTimer.OnComplete += () => { m_InvincibleLock--; };
            m_ChangeSceneTimer.ForceComplete();
            m_Collider = GetComponent<CapsuleCollider2D>();
            m_ObstacleMask = LayerMask.GetMask("Ground");
        }


        protected void Update()
        {
            GetMoveInput();
            GetFireInput(Time.deltaTime);
            Flip();
        }

        public void ChangeWeapon()
        {
            m_CurrentWeaponState = m_CurrentWeaponState == EWeapon.ShotGun ? EWeapon.Bow : EWeapon.ShotGun;
            PlayAnim(EPlayerAnim.Idle);
            m_ChangeSceneTimer.Restart();
            int cache = m_CurrentWeaponIndex;
            m_CurrentWeaponIndex = m_CurrentWeaponIndex - 1 >= 0 ? m_CurrentWeaponIndex - 1 : m_Weapons.Count - 1;

            m_Weapons[cache].gameObject.SetActive(false);
            m_CurrentWeapon = m_Weapons[m_CurrentWeaponIndex];
            m_CurrentWeapon.ChangeDirection();
            m_CurrentWeapon.gameObject.SetActive(true);
        }

        public bool CanChangeWeapon()
        {
            return m_ChangeSceneTimer.Completed;
        }

        private void GetMoveInput()
        {
            float xDirection = Input.GetAxisRaw("Horizontal");
            float yDirection = Input.GetAxisRaw("Vertical");
            m_MoveDirection = new Vector2(xDirection, yDirection);
            m_MoveDirection = m_MoveDirection.normalized;
            m_Rigidbody.velocity = m_MoveDirection * m_MoveSpeed;
        }

        private void Flip()
        {
            float angle = m_CurrentWeapon.transform.eulerAngles.z;
            if ((angle <= 90 && angle >= 0) || (angle >= 270 && angle <= 360))
            {
                m_SpriteRenderer.flipX = false;
            }
            else
                m_SpriteRenderer.flipX = true;
        }

        private void GetFireInput(float deltaTime)
        {
            if (Input.GetMouseButton(0))
            {
                m_CurrentWeapon.Charge(deltaTime);
            }

            if (Input.GetMouseButtonUp(0))
            {
                // var teleport=Instantiate(TeleportAnim);
                // Vector3 angles = m_CurrentWeapon.transform.eulerAngles;
                // angles.z += 180;
                // teleport.transform.eulerAngles =angles;
                // teleport.transform.position = transform.position;
                // transform.localScale=new Vector3(1,)
                if (m_CurrentWeapon != null)
                    m_CurrentWeapon.Fire(this);
            }
        }

        public void Teleport(Vector2 position)
        {
            if (m_IMoveTowards is not null)
            {
                StopCoroutine(m_IMoveTowards);
                if (m_IsMovingToward)
                    m_InvincibleLock--;
            }

            m_IMoveTowards = StartCoroutine(MoveToPosition(position));
        }

        IEnumerator MoveToPosition(Vector2 target)
        {
            int count = 0;
            m_IsMovingToward = true;
            m_InvincibleLock++;
            while (Vector2.Distance(transform.position, target) > 1e-3f && count++ < 20)
            {
                transform.position = Vector2.MoveTowards(transform.position, target, 1);
                yield return new WaitForFixedUpdate();
            }

            m_IsMovingToward = false;
            m_InvincibleLock--;
        }

        public void CauseRecoil(RecoilData data, CinemachineImpulseSource source = null)
        {
            if (m_IRecoil != null) StopCoroutine(m_IRecoil);
            PlayAnim(EPlayerAnim.Recoil);
            StartCoroutine(Recoil(data, source));
        }

        private IEnumerator Recoil(RecoilData data, CinemachineImpulseSource source = null)
        {
            // var source = GetComponent<CinemachineImpulseSource>();
            if (source is not null)
                source.GenerateImpulse();
            for (int i = 1; i <= 2; i++)
            {
                SafeTranslate(-data.FireDirection / 2 * data.RecoilValue);
                SafeTranslate(Vector3.up / 20 * 0.5f);
                yield return new WaitForFixedUpdate();
            }

            for (int i = 1; i <= 6; i++)
            {
                SafeTranslate(-data.FireDirection / 6 * data.RecoilValue);
                SafeTranslate(Vector3.up / 60 * 0.5f);
                yield return new WaitForFixedUpdate();
            }

            PlayAnim(EPlayerAnim.RecoilToIdle);
        }

        private enum EPlayerAnim
        {
            Idle,
            Recoil,
            RecoilToIdle
        }

        private void PlayAnim(EPlayerAnim anim)
        {
            if (anim == EPlayerAnim.Idle)
            {
                string animName = m_CurrentWeaponState == EWeapon.ShotGun ? "ShotGunIdle" : "BowIdle";
                m_Animator.Play(animName);
            }
            else if (anim == EPlayerAnim.RecoilToIdle)
            {
                string animName = m_CurrentWeaponState == EWeapon.ShotGun ? "ShotGunRecoilToIdle" : "BowRecoilToIdle";
                m_Animator.Play(animName);
            }
            else if (anim == EPlayerAnim.Recoil)
            {
                m_Animator.Play("ShotGunRecoil");
            }
        }

        protected void SafeTranslate(Vector2 direction)
        {
            var hit = Physics2D.Raycast(transform.position, direction, direction.magnitude + m_Collider.size.x,
                m_ObstacleMask);
            if (hit.collider is null)
                transform.Translate(direction);
            else
            {
                transform.Translate(direction.normalized * m_Collider.size.x / 10);
            }
        }

        protected void OnDead()
        {
            Instantiate(DieAnim, transform.position, Quaternion.identity);
            gameObject.SetActive(false);
        }
        public void OnAttacked(AttackData data)
        {
            if (Invincible) return;
            m_PlayerStatusInfo.Hp -= data.Damage;
            Log.Info($"Player Hp:{m_PlayerStatusInfo.Hp}");
            if (m_PlayerStatusInfo.IsDead)
            {
                OnDead();
                return;
            }
            m_InvincibleLock++;
            m_InvincibleTimer.Restart();
            Collider2D[] targetEnemies = Physics2D.OverlapCircleAll(transform.position,
                2f, LayerMask.GetMask("Enemy"));
            foreach (var e in targetEnemies)
            {
                e.GetComponent<Enemy>().GetBeaten((e.transform.position - transform.position).normalized);
            }

            CauseRecoil(new RecoilData(data.AttackDirection, 0.2f, 0));
        }
    }
}