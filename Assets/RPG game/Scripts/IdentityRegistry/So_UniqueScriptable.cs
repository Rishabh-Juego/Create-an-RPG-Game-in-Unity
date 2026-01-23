using UnityEngine;
using UnityEngine.Serialization;

namespace TGL.RPG.IdentityRegistry
{
    public abstract class So_UniqueScriptable : ScriptableObject, IUniqueId
    {
        [Space(10), Header("Implement UniqueId"), Space(5)]
        protected string id;
        [FormerlySerializedAs("itemType")] [SerializeField] protected UniqueType uniqueType = UniqueType.Undefined;


        public string UniqueID => id;
        public virtual UniqueType UniqueType => uniqueType;
    }
}
