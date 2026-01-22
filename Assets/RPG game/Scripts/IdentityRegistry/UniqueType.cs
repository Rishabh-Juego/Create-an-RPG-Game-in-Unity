using UnityEngine;

namespace TGL.RPG.IdentityRegistry
{
    public enum UniqueType
    {
        Undefined = 0,
        playerCharacter = 1, // used in scriptable objects to identify the main player character prefabs
        Item = 2, // used in scriptable objects to identify the items in inventory
        Magic = 3, // used in scriptable objects to identify magic abilities
        Spell = 4, // used in scriptable objects to identify spell abilities
    }
}
