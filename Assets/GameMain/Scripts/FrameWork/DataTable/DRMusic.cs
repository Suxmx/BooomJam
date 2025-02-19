﻿using System.IO;
using System.Text;
using GameFramework.Extensions;
using UnityGameFramework.Runtime;

namespace GameMain
{
    /// <summary>
    /// 音乐配置表。
    /// </summary>
    public class DRMusic : DataRowBase
    {
        private int m_Id = 0;

        /// <summary>
        /// 获取音乐编号。
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

        public override bool ParseDataRow(string dataRowString, object userData)
        {
            var columnStrings = dataRowString.Split(',');

            int index = 0;
            m_Id = int.Parse(columnStrings[index++]);
            AssetName = columnStrings[index];

            GeneratePropertyArray();
            return true;
        }

        public override bool ParseDataRow(byte[] dataRowBytes, int startIndex, int length, object userData)
        {
            using (MemoryStream memoryStream = new MemoryStream(dataRowBytes, startIndex, length, false))
            {
                using (BinaryReader binaryReader = new BinaryReader(memoryStream, Encoding.UTF8))
                {
                    m_Id = binaryReader.Read7BitEncodedInt32();
                    AssetName = binaryReader.ReadString();
                }
            }

            GeneratePropertyArray();
            return true;
        }

        private void GeneratePropertyArray()
        {

        }
    }
}