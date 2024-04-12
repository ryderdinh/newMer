using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "Database", menuName = "Database", order = 0)]
    public class Database : ScriptableObject
    {
        public List<CardDatatype> cardData;
    }
}