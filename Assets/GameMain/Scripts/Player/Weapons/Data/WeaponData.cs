using System;
using UnityEngine;

namespace GameMain
{
    /// <summary>
    /// 在游戏流程中打包给武器的数据包
    /// </summary>
    public abstract class WeaponData
    {
        public string name;
        public int Damage;
        public float FireInterval;
        public float MaxChargeTime;
        public GameObject WeaponPrefab;

    }

    public static class WeaponFactory
    {
        public static WeaponData GetWeaponData(DRWeapon drWeapon)
        {
            if (drWeapon.LogicType == typeof(ShotGun))
            {
                return new ShotGunData(drWeapon);
            }
            else //BOW
            {
                return new BowData(drWeapon);
            }
        }
    }
}