using UnityEngine;


namespace TGL.RPG.Data.Character
{
    [CreateAssetMenu(fileName = "CharacterInfo", menuName = "Scriptable Objects/CharacterInfo")]
    public class CharacterInfo : ScriptableObject
    {
        /// <summary>
        /// unique ID of the character
        /// </summary>
        public int characterID; 
        /// <summary>
        /// Default name of the character
        /// </summary>
        public string characterName;
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
    }
}
