using UnityEngine;

namespace GameMain
{
    public class RecoilData 
    {
        public Vector2 FireDirection;
        public float RecoilValue;
        public float Jump;
        
        public RecoilData(Vector2 fireDirection, float recoilValue,float jump)
        {
            FireDirection = fireDirection;
            RecoilValue = recoilValue;
            Jump = jump;
        }
    }
}