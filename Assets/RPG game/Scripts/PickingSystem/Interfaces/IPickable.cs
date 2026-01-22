using TGL.RPG.IdentityRegistry;
using UnityEngine;

namespace TGL.RPG.Items.PickingSystem
{
    public interface IPickable
    {
        void PickUp(IPicker interactor);
        string GetItemName();
        GameObject GetObject();
        Vector3 GetObjectPosition();
        So_UniqueScriptable GetObjectData();
        int GetObjectCount();
    }
}
