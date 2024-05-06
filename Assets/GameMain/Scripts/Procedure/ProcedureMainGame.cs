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
        private ProcedureOwner m_Owner;

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            m_GameBase = new GameBase(GameMode.Level, 0);
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

        public void ReturnToMenu()
        {
            m_Owner.SetData<VarInt32>("NextSceneId", GameEntry.Config.GetInt("Scene.Menu"));
            ChangeState<ProcedureChangeScene>(m_Owner);
        }
    }
}