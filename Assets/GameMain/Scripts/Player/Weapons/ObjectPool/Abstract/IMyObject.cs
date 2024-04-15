using System;

namespace GameMain
{
    public interface IMyObject<T>
    {
        public void OnInit(object userData);
        public void OnShow(object userData);
        public Action<T> RecycleAction { get; set; }
        public void RecycleSelf();
    }
}