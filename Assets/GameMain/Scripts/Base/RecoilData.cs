using UnityEngine;

namespace GameMain
{
    public class RecoilData 
    {
        public Vector2 FireDirection;
        public float RecoilValue;
        
        public RecoilData(Vector2 fireDirection,float recoilValue)
        {
            FireDirection = fireDirection;
            RecoilValue = recoilValue;
        }
    }
}