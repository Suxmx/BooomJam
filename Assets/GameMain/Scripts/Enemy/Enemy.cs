using System;
using System.Collections.Generic;
using GameFramework.Fsm;
using GameMain.States;
using Pathfinding;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class Enemy : MonoBehaviour, IAttackable//,IMyObject
    {
        protected IFsm<Enemy> m_Fsm;
        protected List<FsmState<Enemy>> m_States;
        protected AIPath m_AIPath;
        protected AIDestinationSetter m_AIDestinationSetter;
        protected CharacterStatusInfo m_StatusInfo;
        protected Animator m_Animator;
        protected Rigidbody2D m_Rigidbody;
        
        protected Player player => GameBase.Instance.GetPlayer();
        public float m_IdleDist = 1;
        public float m_TrackDist = 3;
        
        // public void OnInit(object userData)
        private void Awake()
        {
            //TODO:用EnemyData读取数据
            m_StatusInfo = new CharacterStatusInfo(10, 4);
            m_AIPath = GetComponent<AIPath>();
            m_Animator = transform.Find("Animator").GetComponent<Animator>();
            m_AIDestinationSetter = GetComponent<AIDestinationSetter>();
            m_AIPath.maxSpeed = m_StatusInfo.MoveSpeed;

            m_States = new List<FsmState<Enemy>>() { new IdleState(), new TrackState(),new HurtState() };
        }

       

        // public void OnShow(object userData)
        private void Start()
        {
            m_Fsm = GameEntry.Fsm.CreateFsm<Enemy>(FsmUtility.GetFsmName<Enemy>(), this, m_States);
            m_Fsm.Start<TrackState>();
        }
        public Action<object> RecycleAction { get; set; }
        public void RecycleSelf()
        {
            GameEntry.Fsm.DestroyFsm(m_Fsm);
        }

        /// <summary>
        /// 关闭寻路
        /// </summary>
        public void EnableAIPath()
        {
            m_AIPath.enabled = true;
        }

        /// <summary>
        /// 开启寻路
        /// </summary>
        public void DisableAIPath()
        {
            m_AIPath.enabled = false;
        }

        public void SetAIPathTarget(Transform target)
        {
            m_AIDestinationSetter.target = target;
        }

        public void OnDead()
        {
        }

        public float CalculatePlayerDist()
        {
            return Vector2.Distance(player.transform.position, transform.position);
        }

        public void OnAttacked(AttackData data)
        {
            m_StatusInfo.Hp -= data.Damage;
            ((EnemyState)(m_Fsm.CurrentState)).OnHurt(m_Fsm);
        }

        public void PlayAnim(string animName)
        {
            m_Animator.Play(animName);
        }

        public float GetAnimTime()
        {
            // Log.Info( m_Animator.GetCurrentAnimatorStateInfo(0).);
            return m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        }

    }
}