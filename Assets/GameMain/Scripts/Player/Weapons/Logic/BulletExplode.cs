using System;
using System.Collections;
using System.Collections.Generic;
using GameMain;
using UnityEngine;

public class BulletExplode : MonoBehaviour
{
    private Animator m_Animator;
    private PublicObjectPool m_PublicObjectPool;
    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
        m_PublicObjectPool=GameBase.Instance.GetObjectPool();
    }

    private void OnEnable()
    {
            m_Animator.Play("Explode");
    }

    public void Recycle()
    {
        m_PublicObjectPool.UnSpawn(gameObject);
    }
}
