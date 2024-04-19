namespace GameMain
{
    public class CharacterStatusInfo : IAttackable
    {
        protected int m_Hp;

        public CharacterStatusInfo(int maxHp, float moveSpeed)
        {
            m_Hp = maxHp;
            m_MaxHp = maxHp;
            m_MoveSpeed = moveSpeed;
        }

        public virtual int Hp
        {
            get => m_Hp;
            set
            {
                m_Hp=value;
            }
        }

        protected int m_MaxHp;

        public virtual int MaxHp
        {
            get => m_MaxHp;
            protected set
            {
                m_MaxHp = value;
            }
        }

        protected float m_MoveSpeed;

        public virtual float MoveSpeed
        {
            get => m_MoveSpeed;
            protected set
            {
                m_MoveSpeed = value;
            }
        }

        public virtual bool IsDead
        {
            get => Hp <= 0;
        }

        public virtual void OnAttacked(AttackData data)
        {
            Hp -= data.Damage;
        }
    }
}