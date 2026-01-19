using System;
using System.Collections.Generic;
using TGL.RPG.Constants.Sample;
using TGL.RPG.Data.Character;
using TMPro;
using UnityEngine;
using CharacterInfo = TGL.RPG.Data.Character.CharacterInfo;

namespace TGL.RPG.Character
{
    public class CharacterSelector : MonoBehaviour
    {
        [SerializeField] private TMP_InputField nameInputField;
        [SerializeField] private Transform characterPosition;
        [SerializeField] private AvailableCharacters allCharacters;
        private GameObject currentCharacter;
        
        private List<CharacterInfo> allAvailableCharacters;
        private int currentCharacterIndex;
        private string playerName;

        #region MonoBehaviourMethods

        private void Awake()
        {
            if (nameInputField == null)
            {
                Debug.LogError("Name Input Field is not assigned in the inspector.", gameObject);
                return;
            }

            LoadAllCharacters();
        }

        #endregion MonoBehaviourMethods

        #region ButtonMethods
        public void NextCharacter()
        {
            currentCharacterIndex++;
            if (currentCharacterIndex >= allAvailableCharacters.Count)
            {
                currentCharacterIndex = 0;
            }
            DisplayCharacter(currentCharacterIndex);
        }
        
        public void PrevCharacter()
        {
            currentCharacterIndex--;
            if (currentCharacterIndex < 0)
            {
                currentCharacterIndex = allAvailableCharacters.Count - 1;
            }
            DisplayCharacter(currentCharacterIndex);
        }
        
        public void ConfirmSelection()
        {
            playerName = nameInputField.text;
            // currentCharacterIndex // char index
            // allAvailableCharacters[currentCharacterIndex].characterID; // char Id
            // allAvailableCharacters[currentCharacterIndex].characterName; // char name
            
            throw new NotImplementedException();
        }
        #endregion ButtonMethods

        private void LoadAllCharacters()
        {
            allAvailableCharacters?.Clear();
            if (allCharacters == null)
            {
                // load from scriptable object path
                allCharacters = Resources.Load<AvailableCharacters>(GameConstants.ScriptableConstants.gameCharactersPath);
                if (allCharacters == null)
                {
                    Debug.LogError("Failed to load AvailableCharacters ScriptableObject from Resources!");
                    return;
                }
                else
                {
                    Debug.Log(allCharacters.characters.Count);
                }

            }
            allAvailableCharacters = allCharacters.characters;
            currentCharacterIndex = 0;
            DisplayCharacter(currentCharacterIndex);
        }
        
        private void DisplayCharacter(int index)
        {
            // validate
            if (allAvailableCharacters == null || allAvailableCharacters.Count == 0)
            {
                Debug.LogError("No available characters to display.");
                return;
            }
            if (index < 0 || index >= allAvailableCharacters.Count)
            {
                Debug.LogError("Character index out of range.");
                return;
            }
            
            // Clear previous character model
            if (currentCharacter != null)
            {
                Destroy(currentCharacter);
            }
            
            // Instantiate new character model
            currentCharacter = Instantiate(allAvailableCharacters[index].modelPrefab, characterPosition);
            currentCharacter.transform.localPosition = Vector3.zero;
            currentCharacter.transform.localRotation = Quaternion.identity;

            if (nameInputField.placeholder is TextMeshProUGUI tmpPlaceholder)
            {
                tmpPlaceholder.text = allAvailableCharacters[index].characterName;
            }
            else
            {
                Debug.LogWarning("Placeholder is not a TextMeshProUGUI component!");
            }
            nameInputField.text = string.Empty;
            playerName = allAvailableCharacters[index].characterName;

        }
    }
}
