using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using GameFramework.Event;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class Player : EntityLogic
    {
        private SpriteRenderer m_SpriteRenderer;
        private Rigidbody2D m_Rigidbody;
        private Vector3 m_MoveDirection;
        private List<WeaponBase> m_Weapons;
        private WeaponBase m_CurrentWeapon;
        private PlayerStatusInfo m_PlayerStatusInfo;
        private Image m_HpImage;
        private int m_CurrentWeaponIndex;
        private float m_ChangeSceneInterval;
        private float m_ChangeSceneTimer;

        private int m_WeaponToLoad;
        private bool m_Inited;

        private float m_MoveSpeed => m_PlayerStatusInfo.MoveSpeed;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            PlayerData data = (PlayerData)userData;
            m_PlayerStatusInfo = new PlayerStatusInfo(data.MaxHp, data.MoveSpeed);
            m_ChangeSceneInterval = data.ChangeSceneInterval;
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

            m_ChangeSceneTimer = m_ChangeSceneInterval;
        }

        protected void Update()
        {
            if (m_Inited == false || GameBase.Instance.Inited == false) return;
            m_ChangeSceneTimer += Time.deltaTime;

            GetMoveInput();
            GetFireInput(Time.deltaTime);
        }

        public void ChangeWeapon()
        {
            m_ChangeSceneTimer = 0;
            int cache = m_CurrentWeaponIndex;
            m_CurrentWeaponIndex = m_CurrentWeaponIndex - 1 >= 0 ? m_CurrentWeaponIndex - 1 : m_Weapons.Count - 1;

            m_Weapons[cache].Entity.Logic.Visible = false;
            m_CurrentWeapon = m_Weapons[m_CurrentWeaponIndex];
            m_CurrentWeapon.ChangeDirection();
            m_CurrentWeapon.Entity.Logic.Visible = true;
        }

        public bool CanChangeWeapon()
        {
            if (m_ChangeSceneTimer < m_ChangeSceneInterval) return false;
            return true;
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
            //TODO:换枪
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

        public virtual void RecoilForce()
        {
            var source = GetComponent<CinemachineImpulseSource>();
            source.GenerateImpulse(new Vector3(100, 100, 100));
        }
        
        public IEnumerator Recoil(RecoilData data)
        {
            for (int i = 1; i <= 2; i++)
            {
                transform.Translate(-data.FireDirection * data.RecoilValue );
                yield return new WaitForFixedUpdate();
            }

            for (int i = 1; i <= 6; i++)
            {
                transform.Translate(-data.FireDirection * data.RecoilValue);
                yield return new WaitForFixedUpdate();
            }
        }
    }
}