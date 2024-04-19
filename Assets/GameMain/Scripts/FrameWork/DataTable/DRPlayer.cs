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
        public List<int> WeaponDataIds;

        public override bool ParseDataRow(string dataRowString, object userData)
        {
            var columnStrings = dataRowString.Split(',');

            int index = 0;
            m_Id = int.Parse(columnStrings[index++]);
            MaxHp = int.Parse(columnStrings[index++]);
            Damage = int.Parse(columnStrings[index++]);
            MoveSpeed = float.Parse(columnStrings[index++]);
            ChangeSceneInterval = float.Parse(columnStrings[index++]);
            WeaponDataIds = new List<int>();
            for (; index < columnStrings.Length; index++)
            {
                if (!string.IsNullOrEmpty(columnStrings[index]))
                    WeaponDataIds.Add(int.Parse(columnStrings[index]));
            }

            return true;
        }
    }
}