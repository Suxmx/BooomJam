using System;
using System.Collections;
using System.Collections.Generic;
using GameMain;
using GameMain.Scripts.Player.Weapons.ObjectPool;
using UnityEngine;
using UnityGameFramework.Runtime;

public class PublicObjectPool : MonoBehaviour
{
    private Dictionary<string, Queue<GameObject>> m_Pools = new();
    private Dictionary<string, GameObject> m_Template = new();


    public void RegisterTemplate(string name, GameObject template)
    {
        m_Template.Add(name, template);
        m_Pools.Add(name, new Queue<GameObject>());
    }

    /// <summary>
    /// 不要修改Spawn出的物体的名字
    /// </summary>
    /// <param name="name">预先注册的模版的名称</param>
    /// <returns></returns>
    public GameObject Spawn(string name)
    {
        if (!m_Template.ContainsKey(name))
        {
            Log.Error($"没有名称为{name}的预制体，请先用Register方法注册");
        }

        if (m_Pools[name].Count == 0)
        {
            var obj = Instantiate(m_Template[name]);
            obj.name = name + "_Clone";
            return obj;
        }
        else
        {
            var obj= m_Pools[name].Dequeue();
            obj.SetActive(true);
            return obj;
        }
    }

    public void UnSpawn(GameObject obj)
    {
        string name = obj.name.Split("_")[0];
        if (!m_Pools.ContainsKey(name))
        {
            Log.Error($"没有名称为{name}的对象池，请先用Register方法注册");
        }
        obj.gameObject.SetActive(false);
        m_Pools[name].Enqueue(obj);
    }
}