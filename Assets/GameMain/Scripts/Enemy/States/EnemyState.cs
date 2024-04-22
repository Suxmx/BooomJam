using GameFramework.Fsm;

namespace GameMain.States
{
    public abstract class EnemyState : FsmState<Enemy>
    {
        public abstract void OnHurt(IFsm<Enemy> fsm);
    }
}