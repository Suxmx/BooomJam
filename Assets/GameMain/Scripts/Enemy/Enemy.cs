using System;
using System.Collections.Generic;
using GameFramework.Fsm;
using GameMain.States;
using Pathfinding;
using UnityEngine;

namespace GameMain
{
    public class Enemy : MonoBehaviour,IAttackable
    {
        protected IFsm<Enemy> m_Fsm;
        protected List<FsmState<Enemy>> m_States;
        protected AIPath m_AIPath;
        protected AIDestinationSetter m_AIDestinationSetter;
        protected CharacterStatusInfo m_StatusInfo;
        protected Player player=>GameBase.Instance.GetPlayer();
        public float m_IdleDist=1;
        public float m_TrackDist = 3;

        private void Awake()
        {
            m_StatusInfo = new CharacterStatusInfo(10, 4);
            m_AIPath = GetComponent<AIPath>();
            m_AIDestinationSetter = GetComponent<AIDestinationSetter>();
            m_AIPath.maxSpeed = m_StatusInfo.MoveSpeed;
            
            m_States = new List<FsmState<Enemy>>() { new IdleState(), new TrackState() };
        }

        private void Start()
        {
            
            m_Fsm = GameEntry.Fsm.CreateFsm<Enemy>(FsmUtility.GetFsmName<Enemy>(), this,m_States);
            m_Fsm.Start<TrackState>();
        }

        public void EnableAIPath()
        {
            m_AIPath.enabled = true;
        }

        public void DisableAIPath()
        {
            m_AIPath.enabled = false;
        }

        public void SetAIPathTarget(Transform target)
        {
            m_AIDestinationSetter.target = target;
        }

        public float CalculatePlayerDist()
        {
            return Vector2.Distance(player.transform.position, transform.position);
        }
        public void OnAttacked(AttackData data)
        {
            
        }
    }
}