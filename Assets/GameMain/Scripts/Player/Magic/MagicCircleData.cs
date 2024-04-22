using System.Collections;
using System.Collections.Generic;
using GameMain;
using UnityEngine;

namespace GameMain
{
    public class MagicCircleData : EntityData
    {
        public float TimeLimit;
        public float Radius;
        public float DirectionX;
        public float DirectionY;

        public MagicCircleData(int entityId, int typeId, float timeLimit,float radius,float directionX,float directionY) : base(entityId, typeId)
        {
            TimeLimit = timeLimit;
            Radius = radius;
            DirectionX = directionX;
            DirectionY = directionY;
        }
        
    }
}
