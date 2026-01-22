using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TGL.RPG.Data.Character
{
    [CreateAssetMenu(fileName = "AvailableCharacters", menuName = "Scriptable Objects/AvailableCharacters")]
    public class So_AvailableCharacters : ScriptableObject
    {
        public List<So_CharacterInfo> characters;
        
        private void OnValidate()
        {
            if (characters == null || characters.Count == 0) return;
        }
    }
}
