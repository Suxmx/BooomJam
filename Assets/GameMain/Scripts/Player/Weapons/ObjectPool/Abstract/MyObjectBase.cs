﻿using GameFramework;
using GameFramework.ObjectPool;
using UnityEngine;

namespace GameMain.Scripts.Player.Weapons.ObjectPool
{
    public class MyObjectBase : ObjectBase
    {
        public MyObjectBase()
        {
            
        }

        public void SetTarget(object target)
        {
            Initialize(target);
        }

        protected override void Release(bool isShutdown)
        {
            MonoBehaviour mono = (MonoBehaviour)Target;
            if(mono==null)return;
            Object.Destroy(mono.gameObject);
        }
    }
}