using UnityEngine;

namespace GameMain
{
    public class RecoilData 
    {
        public Vector2 FireDirection;
        public float RecoilCoefficient;
        public float ChargeTime;
        public float RecoilValue;
        
        public RecoilData(Vector2 fireDirection, float recoilCoefficient,float chargeTime)
        {
            FireDirection = fireDirection;
            RecoilCoefficient = recoilCoefficient;
            ChargeTime = chargeTime;
            RecoilValue = RecoilCoefficient * ChargeTime;
        }
    }
}