using UnityEngine;

namespace TGL.RPG.IdentityRegistry
{
    public static class ItemRegistry
    {
        /// <summary>
        /// if the passed item has a duplicate ID in the project, returns true
        /// </summary>
        /// <param name="item">the interface that has all data that makes an object unique</param>
        /// <returns>status if the passed data is duplicate</returns>
        public static bool CheckIsDuplicate(UniqueScriptable item)
        {
            if (string.IsNullOrEmpty(item.UniqueID)) return true;

            // Find all ItemData assets in the project
            UniqueScriptable[] allItems = Resources.FindObjectsOfTypeAll<UniqueScriptable>();

            foreach (UniqueScriptable otherItem in allItems)
            {
                // Don't compare the item to itself
                if (ReferenceEquals(otherItem, item)) continue;

                if (otherItem.UniqueID == item.UniqueID)
                {
                    Debug.LogError($"[ID CONFLICT] Found another item with id {otherItem.UniqueID}", otherItem);
                    Debug.LogError($"[ID CONFLICT] The ID '{item.UniqueID}' is already used by {otherItem.name}! Please use a unique ID.", item);
                    return true;
                }
            }
            return false;
        }
    }
}
