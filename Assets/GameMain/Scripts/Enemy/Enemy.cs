using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework.Fsm;
using Pathfinding;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class Enemy : MonoBehaviour, IAttackable, IMyObject
    {
        protected IFsm<Enemy> m_Fsm;
        protected List<FsmState<Enemy>> m_States;
        protected AIPath m_AIPath;
        protected AIDestinationSetter m_AIDestinationSetter;
        protected CharacterStatusInfo m_StatusInfo;
        protected Animator m_Animator;
        protected Rigidbody2D m_Rigidbody;
        protected Collider2D m_Collider;
        protected SpriteRenderer m_SpriteRenderer;
        protected bool spawnSuccess = false;
        protected bool recycled = false;
        protected EnemySpawner m_Spawner;
        protected string m_Name;

        protected Player player => GameBase.Instance.GetPlayer();
        public float m_IdleDist = 1;
        public float m_TrackDist = 3;

        private void Update()
        {
            if (m_AIPath.velocity.x > 0)
            {
                m_SpriteRenderer.flipX = false;
            }
            else if (m_AIPath.velocity.x < 0) m_SpriteRenderer.flipX = true;
        }

        public void OnInit(object userData)
        {
            //TODO:用EnemyData读取数据
            m_Collider = GetComponent<Collider2D>();
            m_Spawner = (EnemySpawner)userData;
            m_StatusInfo = new CharacterStatusInfo(1, 2);
            m_AIPath = GetComponent<AIPath>();
            m_Animator = GetComponent<Animator>();
            m_AIDestinationSetter = GetComponent<AIDestinationSetter>();
            m_AIPath.maxSpeed = m_StatusInfo.MoveSpeed;
            m_SpriteRenderer = GetComponent<SpriteRenderer>();
            m_Rigidbody = GetComponent<Rigidbody2D>();
        }

        public virtual void OnShow(object userData)
        {
            m_Collider.enabled = false;
            SetAIPathTarget(GameBase.Instance.GetPlayer().transform);
            DisableAIPath();
            PlayAnim("Spawn");
            spawnSuccess = false;
            recycled = false;
        }

        public Action<object> RecycleAction { get; set; }

        public virtual void RecycleSelf()
        {
            if (!recycled)
            {
                GameBase.Instance.OnEnemyDie();
                recycled = true;
                m_Spawner.Unspawn(this);
            }
        }

        public void SetName(string name)
        {
            m_Name = name;
        }

        public string GetName()
        {
            return m_Name;
        }


        public virtual void OnSpawnSuccess()
        {
            spawnSuccess = true;
            EnableAIPath();
            m_Collider.enabled = true;
        }

        /// <summary>
        /// 关闭寻路
        /// </summary>
        protected void EnableAIPath()
        {
            // m_AIPath = GetComponent<AIPath>();
            m_AIPath.enabled = true;
        }

        /// <summary>
        /// 开启寻路
        /// </summary>
        protected void DisableAIPath()
        {
            m_AIPath.enabled = false;
        }

        protected void SetAIPathTarget(Transform target)
        {
            m_AIDestinationSetter.target = target;
        }

        public virtual void OnDead()
        {
            if (!spawnSuccess) return;
            DisableAIPath();
            m_Animator.Play("Die");
            m_Collider.enabled = false;
            m_Rigidbody.velocity = Vector2.zero;
            // Destroy(gameObject);
        }

        public virtual void OnAttacked(AttackData data)
        {
            OnDead();
        }

        public void PlayAnim(string animName)
        {
            m_Animator.Play(animName);
        }

        public virtual void GetBeaten(Vector2 force)
        {
            DisableAIPath();
            StartCoroutine(Recoil(force));
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                other.gameObject.GetComponent<IAttackable>()
                    .OnAttacked(new AttackData(1, transform.position - other.transform.position));
            }
        }

        IEnumerator Recoil(Vector2 direction)
        {
            transform.right = Vector2.right;
            for (int i = 1; i <= 2; i++)
            {
                SafeTranslate(direction / 2 * 0.8f);
                yield return new WaitForFixedUpdate();
            }

            for (int i = 1; i <= 6; i++)
            {
                SafeTranslate(direction / 6 * 0.8f);
                yield return new WaitForFixedUpdate();
            }

            EnableAIPath();
        }

        protected void SafeTranslate(Vector2 direction)
        {
            var m_ObstacleMask = LayerMask.GetMask("Ground");
            var hit = Physics2D.Raycast(transform.position, direction, direction.magnitude + GetColliderSize(),
                m_ObstacleMask);
            if (hit.collider is null)
                transform.Translate(direction);
            else
            {
                transform.Translate(direction.normalized * GetColliderSize() / 10);
            }
        }

        protected float GetColliderSize()
        {
            if (m_Collider is CapsuleCollider2D)
            {
                return (m_Collider as CapsuleCollider2D).size.x;
            }
            else if (m_Collider is CircleCollider2D)
            {
                return (m_Collider as CircleCollider2D).radius;
            }
            else
            {
                Log.Error($"Enemy使用了未注册碰撞体类型{(m_Collider.GetType())}");
                return 0;
            }
        }
    }
}