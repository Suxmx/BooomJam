using System.Collections.Generic;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class DRPlayer : DataRowBase
    {
        private int m_Id = 0;

        /// <summary>
        /// 获取实体编号。
        /// </summary>
        public override int Id
        {
            get { return m_Id; }
        }

        public int MaxHp;
        public int Damage;
        public float MoveSpeed;
        public float ChangeSceneInterval;
        public List<string> WeaponNames;

        public override bool ParseDataRow(string dataRowString, object userData)
        {
            var columnStrings = dataRowString.Split(',');

            int index = 0;
            m_Id = int.Parse(columnStrings[index++]);
            MaxHp = int.Parse(columnStrings[index++]);
            Damage = int.Parse(columnStrings[index++]);
            MoveSpeed = float.Parse(columnStrings[index++]);
            ChangeSceneInterval = float.Parse(columnStrings[index++]);
            WeaponNames = new List<string>();
            for (; index < columnStrings.Length; index++)
            {
                if (!string.IsNullOrEmpty(columnStrings[index]))
                    WeaponNames.Add(columnStrings[index]);
            }

            return true;
        }
    }
}