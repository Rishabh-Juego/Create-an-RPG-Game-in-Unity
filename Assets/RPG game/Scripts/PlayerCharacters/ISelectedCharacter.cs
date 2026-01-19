using UnityEngine;
using CharacterInfo = TGL.RPG.Data.Character.CharacterInfo;

namespace TGL.RPG.Character
{
    public interface ISelectedCharacter
    {
        CharacterInfo SelectedCharacter { get;}
        string SelectedCharacterName { get;}
    }
}
