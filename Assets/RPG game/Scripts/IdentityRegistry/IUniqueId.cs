using UnityEngine;

namespace TGL.RPG.IdentityRegistry
{
    public interface IUniqueId
    {
        public string UniqueID { get; }
        public UniqueType UniqueType { get; }
    }
}