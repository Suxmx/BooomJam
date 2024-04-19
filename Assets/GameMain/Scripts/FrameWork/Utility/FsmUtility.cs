using System;
using System.Collections.Generic;

namespace GameMain
{
    public static class FsmUtility
    {
        private static Dictionary<Type, int> m_TypeCountDict=new ();

        public static string GetFsmName<T>()
        {
            string typeName = typeof(T).ToString();
            if(!m_TypeCountDict.ContainsKey(typeof(T)))
                m_TypeCountDict.Add(typeof(T),0);
            typeName += m_TypeCountDict[typeof(T)].ToString();
            m_TypeCountDict[typeof(T)]++;
            return typeName;
        }
    }
}