using System;
using System.Collections.Generic;
using UnityGameFramework.Runtime;

namespace GameMain
{
    /// <summary>
    /// 从CSV文件中读取出来的数据配表
    /// </summary>
    public class DRWeapon : DataRowBase
    {
        public static Dictionary<string, int> WeaponName2Id = new Dictionary<string, int>()
        {
            {"ShotGun",(int)EWeapon.ShotGun},
            {"Bow",(int)EWeapon.Bow}
        };
        private int m_Id = 0;

        public override int Id
        {
            get { return m_Id; }
        }

        public Type LogicType;
        public List<int> IntParams;
        public List<float> FloatParams;

        //为了适应多种不同的武器进行的妥协，建议到时候仔细对一下Index对应的数据
        public override bool ParseDataRow(string dataRowString, object userData)
        {
            IntParams = new List<int>();
            FloatParams = new List<float>();
            var columnStrings = dataRowString.Split(',');
            m_Id = int.Parse(columnStrings[0]);
            string typeName = "GameMain." + columnStrings[1];
            LogicType = Type.GetType(typeName);
            for (int index = 2; index < columnStrings.Length; index++)
            {
                if (columnStrings[index].Contains('f'))
                {
                    FloatParams.Add(float.Parse(columnStrings[index].Substring(1)));
                }
                else if (columnStrings[index].Contains('i'))
                    IntParams.Add(int.Parse(columnStrings[index].Substring(1)));
            }

            return true;
        }
    }
}