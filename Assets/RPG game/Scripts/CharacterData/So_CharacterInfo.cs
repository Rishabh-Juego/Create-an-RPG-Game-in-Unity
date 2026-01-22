using System;
using TGL.RPG.IdentityRegistry;
using UnityEngine;


namespace TGL.RPG.Data.Character
{
    [CreateAssetMenu(fileName = "CharacterInfo", menuName = "Scriptable Objects/CharacterInfo")]
    public class CharacterInfo : UniqueScriptable
    {
        /// <summary>
        /// Default name of the character
        /// </summary>
        [Space(10), Header("CharacterInfo"), Space(5)]public string characterName;
        /// <summary>
        /// player model with only idle animation
        /// </summary>
        [Tooltip("player model with only idle animation")]public GameObject modelPrefab;
        /// <summary>
        /// Actual player with all necessary scripts
        /// </summary>
        [Tooltip("Actual player with all necessary scripts")]public GameObject playerPrefab;
        /// <summary>
        /// in case we want to have gender discrimination
        /// </summary>
        public bool isMale;

        private void OnValidate()
        {
            ItemRegistry.CheckIsDuplicate(this);
        }
    }
}
