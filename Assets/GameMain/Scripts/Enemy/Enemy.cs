using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework.Fsm;
using Pathfinding;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class Enemy : MonoBehaviour, IAttackable, IMyObject
    {
        protected IFsm<Enemy> m_Fsm;
        protected List<FsmState<Enemy>> m_States;
        protected AIPath m_AIPath;
        protected AIDestinationSetter m_AIDestinationSetter;
        protected CharacterStatusInfo m_StatusInfo;
        protected Animator m_Animator;
        protected Rigidbody2D m_Rigidbody;
        protected bool spawnSuccess = false;
        protected bool recycled=false;

        protected Player player => GameBase.Instance.GetPlayer();
        public float m_IdleDist = 1;
        public float m_TrackDist = 3;

        public void OnInit(object userData)
        {
            //TODO:用EnemyData读取数据
            m_StatusInfo = new CharacterStatusInfo(10, 2);
            m_AIPath = GetComponent<AIPath>();
            m_Animator = GetComponent<Animator>();
            m_AIDestinationSetter = GetComponent<AIDestinationSetter>();
            m_AIPath.maxSpeed = m_StatusInfo.MoveSpeed;
        }

        public void OnShow(object userData)
        {
            SetAIPathTarget(GameBase.Instance.GetPlayer().transform);
            DisableAIPath();
            PlayAnim("Spawn");
            spawnSuccess = false;
            recycled = false;
        }

        public Action<object> RecycleAction { get; set; }

        public void RecycleSelf()
        {
            if (!recycled)
            {
                recycled = true;
                gameObject.SetActive(false);
                RecycleAction(this);
            }
        }

        public void OnSpawnSuccess()
        {
            spawnSuccess = true;
            EnableAIPath();
        }

        /// <summary>
        /// 关闭寻路
        /// </summary>
        public void EnableAIPath()
        {
            // m_AIPath = GetComponent<AIPath>();
            m_AIPath.enabled = true;
            m_Animator.SetBool("Run",true);
        }

        /// <summary>
        /// 开启寻路
        /// </summary>
        public void DisableAIPath()
        {
            m_AIPath.enabled = false;
            m_Animator.SetBool("Run",false);
        }

        public void SetAIPathTarget(Transform target)
        {
            m_AIDestinationSetter.target = target;
        }

        public void OnDead()
        {
            if (!spawnSuccess) return;
            m_Animator.SetBool("Dead",false);
            RecycleSelf();
            // Destroy(gameObject);
        }

        public void OnAttacked(AttackData data)
        {
            OnDead();
            m_Animator.SetBool("Dead",true);
        }

        public void PlayAnim(string animName)
        {
            m_Animator.Play(animName);
        }

        public void GetBeaten(Vector2 force)
        {
            DisableAIPath();
            StartCoroutine(Recoil(force));
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                other.gameObject.GetComponent<IAttackable>()
                    .OnAttacked(new AttackData(1, transform.position - other.transform.position));
            }
        }

        IEnumerator Recoil(Vector2 direction)
        {
            transform.right = Vector2.right;
            for (int i = 1; i <= 2; i++)
            {
                transform.Translate(direction / 2 * 0.8f);
                yield return new WaitForFixedUpdate();
            }

            for (int i = 1; i <= 6; i++)
            {
                transform.Translate(direction / 6 * 0.8f);
                yield return new WaitForFixedUpdate();
            }

            EnableAIPath();
        }
    }
}