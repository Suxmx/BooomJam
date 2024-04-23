﻿using UnityEngine;

namespace GameMain
{
    public class AttackData
    {
        public int Damage;
        public Vector2 AttackDirection;

        public AttackData(int damage)
        {
            Damage = damage;
        }

        public AttackData(int damage, Vector2 attackDirection)
        {
            Damage = damage;
            AttackDirection = attackDirection.normalized;
        }
    }
}