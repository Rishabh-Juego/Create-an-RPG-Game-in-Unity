using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TGL.RPG.Data.Character
{
    [CreateAssetMenu(fileName = "AvailableCharacters", menuName = "Scriptable Objects/AvailableCharacters")]
    public class AvailableCharacters : ScriptableObject
    {
        public List<CharacterInfo> characters;
        
        private void OnValidate()
        {
            if (characters == null || characters.Count == 0) return;

            CheckForDuplicateIDs();
        }
        
        private void CheckForDuplicateIDs()
        {
            List<int> duplicates = characters
                .Where(c => c != null)
                .GroupBy(c => c.characterID)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList();

            if (duplicates.Count > 0)
            {
                foreach (var id in duplicates)
                {
                    Debug.LogError($"[AvailableCharacters] Duplicate CharacterID found: {id}. Please ensure all IDs are unique.");
                }
            }
            else
            {
                Debug.Log($"No Duplicate CharacterIDs found in AvailableCharacters '{name}'.", this);
            }
        }
    }
}
