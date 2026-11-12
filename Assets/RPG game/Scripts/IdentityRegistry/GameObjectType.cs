using UnityEngine;

namespace TGL.RPG.IdentityRegistry
{
    public enum GameObjectType
    {
        Undefined = 0,
        playerCharacter = 1, // used in scriptable objects to identify the main player character prefabs
        Inventory = 5, // used to identify inventory systems
    }
}
