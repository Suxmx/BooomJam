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
    public class Player : EntityLogic, IAttackable
    {
        private SpriteRenderer m_SpriteRenderer;
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
        public Lock m_InvincibleLock;
        private int m_CurrentWeaponIndex;
        private int m_ObstacleMask;

        private bool Invincible
        {
            get { return !m_InvincibleLock.Unlocked; }
        }

        private int m_WeaponToLoad;
        private bool m_Inited;

        private float m_MoveSpeed => m_PlayerStatusInfo.MoveSpeed;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            PlayerData data = (PlayerData)userData;
            m_PlayerStatusInfo = new PlayerStatusInfo(data.MaxHp, data.MoveSpeed);
            m_WeaponToLoad = data.WeaponsDatas.Count;
            m_Weapons = new List<WeaponBase>();
            GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, OnShowWeaponSuccess);
            foreach (var weapon in data.WeaponsDatas)
            {
                GameEntry.Entity.ShowWeapon(weapon);
            }

            m_Rigidbody = GetComponent<Rigidbody2D>();
            m_SpriteRenderer = GetComponentInChildren<SpriteRenderer>();
            var hpObj = GameObject.Find("Hp");
            if (hpObj)
            {
                m_HpImage = hpObj.transform.Find("HpInside").GetComponent<Image>();
                m_HpImage.fillAmount = 0;
            }

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
                transform.Find("PlayerImage").GetComponent<SpriteRenderer>().color = Color.red;
            };
            m_InvincibleTimer.OnComplete += () => { m_InvincibleLock--; };
            m_ChangeSceneTimer.ForceComplete();
            m_Collider = GetComponent<CapsuleCollider2D>();
            m_ObstacleMask = LayerMask.GetMask("Ground");
        }

        protected void Update()
        {
            if (m_Inited == false || GameBase.Instance.Inited == false) return;

            GetMoveInput();
            GetFireInput(Time.deltaTime);
        }

        public void ChangeWeapon()
        {
            m_ChangeSceneTimer.Restart();
            int cache = m_CurrentWeaponIndex;
            m_CurrentWeaponIndex = m_CurrentWeaponIndex - 1 >= 0 ? m_CurrentWeaponIndex - 1 : m_Weapons.Count - 1;

            m_Weapons[cache].Entity.Logic.Visible = false;
            m_CurrentWeapon = m_Weapons[m_CurrentWeaponIndex];
            m_CurrentWeapon.ChangeDirection();
            m_CurrentWeapon.Entity.Logic.Visible = true;
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
            int faceDirection = m_SpriteRenderer.flipX ? -1 : 1;
            if ((xDirection < -0.1f && faceDirection == 1) || (xDirection > 0.1f && faceDirection == -1))
            {
                m_SpriteRenderer.flipX = !m_SpriteRenderer.flipX;
            }
        }

        private void GetFireInput(float deltaTime)
        {
            if (Input.GetMouseButton(0))
            {
                m_CurrentWeapon.Charge(deltaTime);
                if (m_HpImage)
                {
                    m_HpImage.fillAmount = m_CurrentWeapon.GetChargePercent();
                    if (Mathf.Abs(m_HpImage.fillAmount - 1) < 1e-5)
                    {
                        m_HpImage.color = new Color(0, 1, 0, 1);
                    }
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (m_CurrentWeapon != null)
                    m_CurrentWeapon.Fire(this);
                if (m_HpImage)
                {
                    m_HpImage.fillAmount = 0;
                    m_HpImage.color = new Color(1, 0, 0, 1);
                }
            }
        }

        public void Teleport(Vector2 position)
        {
            transform.position = position;
        }

        private void OnShowWeaponSuccess(object sender, GameEventArgs e)
        {
            ShowEntitySuccessEventArgs ne = (ShowEntitySuccessEventArgs)e;
            if (ne.EntityLogicType.IsSubclassOf(typeof(WeaponBase)))
            {
                m_Weapons.Add((WeaponBase)ne.Entity.Logic);
                m_WeaponToLoad--;
                GameEntry.Entity.AttachEntity(ne.Entity, Entity, "Weapons");
                ne.Entity.transform.position = Vector3.zero;
                if (m_CurrentWeapon == null)
                {
                    m_CurrentWeaponIndex = 0;
                    m_CurrentWeapon = (WeaponBase)ne.Entity.Logic;
                }
                else ne.Entity.Logic.Visible = false;

                if (m_WeaponToLoad == 0)
                {
                    m_Inited = true;
                    GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, OnShowWeaponSuccess);
                }
            }
        }

        public void CauseRecoil(RecoilData data, CinemachineImpulseSource source = null)
        {
            if (m_IRecoil != null) StopCoroutine(m_IRecoil);
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

        public void OnAttacked(AttackData data)
        {
            if (Invincible) return;
            m_PlayerStatusInfo.Hp -= data.Damage;
            m_InvincibleLock++;
            m_InvincibleTimer.Restart();
            Collider2D[] targetEnemies = Physics2D.OverlapCircleAll(transform.position,
                1f, LayerMask.GetMask("Enemy"));
            Debug.Log(targetEnemies.Length);
            foreach (var e in targetEnemies)
            {
                e.GetComponent<Enemy>().GetBeaten((e.transform.position - transform.position).normalized);
                
            }

            CauseRecoil(new RecoilData(data.AttackDirection, 0.2f, 0));
        }
    }
}