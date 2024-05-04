using GameFramework.DataTable;
using GameFramework.Event;
using GameFramework.Extensions.Sound;
using GameFramework.Procedure;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace GameMain
{
    public class ProcedureLevelSelect : ProcedureBase
    {
        private ProcedureOwner owner;
        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            owner = procedureOwner;
        }
        

        public void SelectLevel(int index)
        {
            owner.SetData<VarInt32>("Level",index-1);
            ChangeState<ProcedurePreloadMainGame>(owner);
        }
    }
}