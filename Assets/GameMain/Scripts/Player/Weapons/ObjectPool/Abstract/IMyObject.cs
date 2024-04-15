using System;

namespace GameMain
{
    public interface IMyObject
    {
        public void OnInit(object userData);
        public void OnShow(object userData);
        public Action<object> RecycleAction { get; set; }
        public void RecycleSelf();
    }
}