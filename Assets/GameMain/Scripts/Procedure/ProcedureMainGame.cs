using GameFramework.DataTable;
using GameFramework.Event;
using GameFramework.Extensions.Sound;
using GameFramework.Procedure;
using MyTimer;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace GameMain
{
    public class ProcedureMainGame : ProcedureBase
    {
        private GameBase m_GameBase;
        private ProcedureOwner m_Owner;

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            
            m_GameBase = new GameBase(GameMode.Level, procedureOwner.GetData<VarInt32>("Level"));
            object data = procedureOwner.GetData<VarObject>("GameData").Value;
            m_GameBase.Init((GameBaseData)data);
            GameBase.Instance = m_GameBase;
            m_Owner = procedureOwner;
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            m_GameBase.Update();
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
        }

        public void ReturnToMenu()
        {
            m_Owner.SetData<VarInt32>("NextSceneId", GameEntry.Config.GetInt("Scene.Menu"));
            ChangeState<ProcedureChangeScene>(m_Owner);
        }
    }
}