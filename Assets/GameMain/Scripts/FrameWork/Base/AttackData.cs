namespace GameMain
{
    public class AttackData
    {
        protected int m_Damage;

        public AttackData(int damage)
        {
            m_Damage = damage;
        }

        public int Damage
        {
            get => m_Damage;
            protected set
            {
                m_Damage = value;
            }
        }
    }
}