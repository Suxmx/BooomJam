﻿using UnityGameFramework.Runtime;

namespace GameMain
{
    /// <summary>
    /// 声音配置表。
    /// </summary>
    public class DRUISound : DataRowBase
    {
        private int m_Id = 0;

        /// <summary>
        /// 获取声音编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return m_Id;
            }
        }

        /// <summary>
        /// 获取资源名称。
        /// </summary>
        public string AssetName
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取优先级（默认0，128最高，-128最低）。
        /// </summary>
        public int Priority
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取音量（0~1）。
        /// </summary>
        public float Volume
        {
            get;
            private set;
        }

        public override bool ParseDataRow(string dataRowString, object userData)
        {
            var columnStrings = dataRowString.Split(',');
            int index = 0;
            m_Id = int.Parse(columnStrings[index++]);
            AssetName = columnStrings[index++];
            Priority = int.Parse(columnStrings[index++]);
            Volume = float.Parse(columnStrings[index]);

            GeneratePropertyArray();
            return true;
        }

        private void GeneratePropertyArray()
        {

        }
    }
}