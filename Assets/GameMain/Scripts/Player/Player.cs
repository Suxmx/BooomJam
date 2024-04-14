using System;
using System.Collections.Generic;
using GameFramework.Event;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public enum EPlayerType : int
    {
        Default = 1000
    }

    public class Player : EntityLogic
    {
        private SpriteRenderer m_SpriteRenderer;
        private Rigidbody2D m_Rigidbody;
        private Vector3 m_MoveDirection;
        private List<WeaponBase> m_Weapons;
        private WeaponBase m_CurrentWeapon;
        private PlayerStatusInfo m_PlayerStatusInfo;

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
            m_SpriteRenderer = GetComponent<SpriteRenderer>();
        }

        protected override void OnRecycle()
        {
            base.OnRecycle();
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            if (m_Inited == false) return;
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            GetMoveInput();
            GetFireInput();
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

        private void GetFireInput()
        {
            //TODO:换枪
            if (Input.GetMouseButtonDown(0))
            {
                m_CurrentWeapon?.Fire(this);
            }
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
                    m_CurrentWeapon = (WeaponBase)ne.Entity.Logic;
                }
                else ne.Entity.Logic.Visible = false;

                if (m_WeaponToLoad == 0)
                {
                    m_Inited = true;
                    GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId,OnShowWeaponSuccess);
                }
            }
        }
    }
}