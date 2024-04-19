using System;
using System.Collections.Generic;
using GameFramework.Fsm;
using GameMain.States;
using Pathfinding;
using UnityEngine;

namespace GameMain
{
    public class Enemy : MonoBehaviour
    {
        protected IFsm<Enemy> m_Fsm;
        protected List<FsmState<Enemy>> m_States;
        protected AIPath m_AIPath;
        protected AIDestinationSetter m_AIDestinationSetter;

        private void Start()
        {
            m_AIPath = GetComponent<AIPath>();
            m_AIDestinationSetter = GetComponent<AIDestinationSetter>();
            
            m_States = new List<FsmState<Enemy>>();
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
    }
}