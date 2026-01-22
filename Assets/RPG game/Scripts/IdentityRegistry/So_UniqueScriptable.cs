using UnityEngine;

namespace TGL.RPG.IdentityRegistry
{
    public abstract class So_UniqueScriptable : ScriptableObject, IUniqueId
    {
        [Space(10), Header("Implement UniqueId"), Space(5)]
        protected string itemName;
        [SerializeField] protected UniqueType itemType = UniqueType.Undefined;


        public string UniqueID => itemName;
        public UniqueType UniqueType => itemType;
    }
}
