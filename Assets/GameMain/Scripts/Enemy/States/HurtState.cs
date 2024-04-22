using System;
using GameFramework.Fsm;
using UnityEngine;

namespace GameMain.States
{
    public class HurtState : EnemyState
    {
        protected override void OnEnter(IFsm<Enemy> fsm)
        {
            base.OnEnter(fsm);
            fsm.Owner.PlayAnim("TestHurt");
        }

        protected override void OnUpdate(IFsm<Enemy> fsm, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
            if(fsm.Owner.GetAnimTime()>1f )
                ChangeState<TrackState>(fsm);
        }

        protected override void OnLeave(IFsm<Enemy> fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);
            // fsm.Owner.
        }

        public override void OnHurt(IFsm<Enemy> fsm)
        {
            
        }
    }
}