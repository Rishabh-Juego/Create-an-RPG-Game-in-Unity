using System;
using System.Collections.Generic;
using App.SceneManagement;
using TGL.RPG.CommunicationBus;
using TGL.RPG.CommunicationBus.Sample;
using TGL.RPG.Constants.Sample;
using TGL.RPG.Data.Character;
using TGL.ServiceLocator;
using TMPro;
using UnityEngine;

namespace TGL.RPG.Character
{
    public class CharacterSelector : MonoBehaviour
    {
        [SerializeField] private TMP_InputField nameInputField;
        [SerializeField] private Transform characterPosition;
        [SerializeField] private So_AvailableCharacters allCharacters;
        private GameObject currentCharacter;
        
        private List<So_CharacterInfo> allAvailableCharacters;
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
        /// <summary>
        /// called by Inspector button to go to the next character
        /// </summary>
        public void NextCharacter()
        {
            currentCharacterIndex++;
            if (currentCharacterIndex >= allAvailableCharacters.Count)
            {
                currentCharacterIndex = 0;
            }
            DisplayCharacter(currentCharacterIndex);
        }
        
        /// <summary>
        /// called by Inspector button to go to the previous character
        /// </summary>
        public void PrevCharacter()
        {
            currentCharacterIndex--;
            if (currentCharacterIndex < 0)
            {
                currentCharacterIndex = allAvailableCharacters.Count - 1;
            }
            DisplayCharacter(currentCharacterIndex);
        }
        
        /// <summary>
        /// called by Inspector button to confirm the selection and proceed to the main game scene
        /// </summary>
        public void ConfirmSelection()
        {
            playerName = nameInputField.text; // In case user has changed the name
            if (string.IsNullOrEmpty(playerName))
            {
                playerName = allAvailableCharacters[currentCharacterIndex].characterName;
            }
            
            // if we have an old registration, unregister it first
            if (SLocator.GetSlGlobal?.HasService<ISelectedCharacter>() ?? false)
            {
                SLocator.GetSlGlobal?.UnRegister(typeof(ISelectedCharacter));
            }
            
            // TODO: Service Locator registration is tightly coupled here for simplicity. In a production scenario, consider using a more decoupled approach.
            SLocator.GetSlGlobal.Register(typeof(ISelectedCharacter), new SelectedCharacterInfoData(allAvailableCharacters[currentCharacterIndex], playerName)); // save the choice made
            MessageBus.PublishMessage(MessageTypes.ActivateSingleScene, new ChangeSceneEvent(AppSceneTypes.MainGameScene)); // change scene
        }
        #endregion ButtonMethods

        private void LoadAllCharacters()
        {
            allAvailableCharacters?.Clear();
            if (allCharacters == null)
            {
                // load from scriptable object path
                allCharacters = Resources.Load<So_AvailableCharacters>(GameConstants.ScriptableConstants.gameCharactersPath);
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
