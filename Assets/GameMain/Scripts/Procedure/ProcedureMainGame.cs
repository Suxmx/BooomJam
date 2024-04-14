using GameFramework.DataTable;
using GameFramework.Event;
using GameFramework.Extensions.Sound;
using GameFramework.Procedure;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace GameMain
{
    public class ProcedureMainGame : ProcedureBase
    {
        private GameBase m_GameBase;
        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            m_GameBase = new GameBase();
            m_GameBase.Init();
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            if (!m_GameBase.Inited) return;
            m_GameBase.Update();
        }
    }
}