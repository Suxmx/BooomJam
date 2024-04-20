using System.Collections;
using UnityEngine;

namespace GameMain
{
    public partial class GameEntry
    {
        public static GameObject InstantiateHelper(GameObject template)
        {
            return Instantiate(template);
        }

    }
}