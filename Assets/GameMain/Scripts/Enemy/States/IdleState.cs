using GameFramework.Fsm;
using UnityGameFramework.Runtime;

namespace GameMain.States
{
    public class IdleState : EnemyState
    {
        protected override void OnEnter(IFsm<Enemy> fsm)
        {
            base.OnEnter(fsm);
        }

        protected override void OnUpdate(IFsm<Enemy> fsm, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
            if(fsm.Owner.CalculatePlayerDist()>fsm.Owner.m_TrackDist)
                ChangeState<TrackState>(fsm);
            
        }

        protected override void OnLeave(IFsm<Enemy> fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);
        }

        public override void OnHurt(IFsm<Enemy> fsm)
        {
            ChangeState<HurtState>(fsm);
        }
    }
}