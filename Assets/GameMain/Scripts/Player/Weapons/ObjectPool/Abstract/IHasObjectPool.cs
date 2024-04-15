using UnityEngine;

namespace GameMain.Scripts.Player.Weapons.ObjectPool
{
    public interface IHasObjectPool
    {
        public GameObject CreateObject();
    }
}