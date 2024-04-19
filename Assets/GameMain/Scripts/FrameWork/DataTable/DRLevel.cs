using UnityGameFramework.Runtime;

namespace GameMain
{
    public class DRLevel : DataRowBase
    {
        private int m_Id = 0;
        
        public override int Id
        {
            get
            {
                return m_Id;
            }
        }

        public string GameScene1;
        public string GameScene2;
        public override bool ParseDataRow(string dataRowString, object userData)
        {
            var columnStrings = dataRowString.Split(',');

            int index = 0;
            m_Id = int.Parse(columnStrings[index++]);
            GameScene1 = columnStrings[index++];
            GameScene2 = columnStrings[index];

            return true;
        }
    }
}