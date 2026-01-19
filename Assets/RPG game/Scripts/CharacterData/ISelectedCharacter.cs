using UnityEngine;
using CharacterInfo = TGL.RPG.Data.Character.CharacterInfo;

namespace TGL.RPG.Character
{
    public interface ISelectedCharacter
    {
        CharacterInfo SelectedCharacter { get;}
        string SelectedCharacterName { get;}
    }

    public class SelectedCharacterInfoData : ISelectedCharacter
    {
        public SelectedCharacterInfoData(CharacterInfo selectedCharacter, string selectedCharacterName)
        {
            SelectedCharacter = selectedCharacter;
            SelectedCharacterName = selectedCharacterName;
        }
        
        public CharacterInfo SelectedCharacter { get; }
        public string SelectedCharacterName { get; }
    }
}
