using UnityEngine;


namespace TGL.RPG.Data.Character
{
    [CreateAssetMenu(fileName = "CharacterInfo", menuName = "Scriptable Objects/CharacterInfo")]
    public class CharacterInfo : ScriptableObject
    {
        public int characterID; 
        public string characterName;
        public GameObject modelPrefab;
        public GameObject playerPrefab;
        public bool isMale;
    }
}
