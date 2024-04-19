using GameFramework.Fsm;

namespace GameMain.States
{
    public class TrackState : FsmState<Enemy>
    {
        protected override void OnEnter(IFsm<Enemy> fsm)
        {
            base.OnEnter(fsm);
            fsm.Owner.EnableAIPath();
            fsm.Owner.SetAIPathTarget(GameBase.Instance.GetPlayer().transform);
        }

        protected override void OnUpdate(IFsm<Enemy> fsm, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
        }

        protected override void OnLeave(IFsm<Enemy> fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);
            fsm.Owner.SetAIPathTarget(null);
            fsm.Owner.DisableAIPath();
        }
    }
}