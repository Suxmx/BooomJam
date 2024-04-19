using System.IO;
using System.Text;
using GameFramework.Extensions;
using UnityGameFramework.Runtime;

namespace GameMain
{
    /// <summary>
    /// 实体表。
    /// </summary>
    public class DREntity : DataRowBase
    {
        private int m_Id = 0;

        /// <summary>
        /// 获取实体编号。
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

        private void GeneratePropertyArray()
        {

        }
    }
}