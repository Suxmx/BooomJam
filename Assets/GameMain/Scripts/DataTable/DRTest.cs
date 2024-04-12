using UnityGameFramework.Runtime;

namespace GameMain
{
    public class DRTest : DataRowBase
    {
        private int m_Id = 0;
        public override int Id
        {
            get
            {
                return m_Id;
            }
        }

        public int Test1
        {
            get;
            private set;
        }
        public float Test2
        {
            get;
            private set;
        }
        public string Test3
        {
            get;
            private set;
        }

        public override bool ParseDataRow(string dataRowString, object userData)
        {
            var columnStrings = dataRowString.Split(',');
            int index = 0;
            m_Id = int.Parse(columnStrings[index++]);
            Test1=int.Parse(columnStrings[index++]);
            Test2 = float.Parse(columnStrings[index++]);
            Test3 = columnStrings[index];

            return true;
        }
    }
}