using TGL.RPG.Data.Character;
using UnityEngine;

namespace TGL.RPG.Character
{
    public interface ISelectedCharacter
    {
        So_CharacterInfo SelectedCharacter { get;}
        string SelectedCharacterName { get;}
    }
}
