using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GMEngine
{
    [CreateAssetMenu(fileName = "Dumb Enemy Database", menuName = "Scriptable Object/Database/Dumb Enemy Database")]
    public class DumbEnemyDatabase : ScriptableObject
    {
        public List<Transform> m_EnemyList;
    }

}

