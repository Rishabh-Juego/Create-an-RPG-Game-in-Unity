using TGL.RPG.Data.Character;
using UnityEngine;

namespace TGL.RPG.Character
{
    public class SelectedCharacterInfoData : ISelectedCharacter
    {
        public SelectedCharacterInfoData(So_CharacterInfo selectedCharacter, string selectedCharacterName)
        {
            SelectedCharacter = selectedCharacter;
            SelectedCharacterName = selectedCharacterName;
        }
        
        public So_CharacterInfo SelectedCharacter { get; }
        public string SelectedCharacterName { get; }
    }
}
