using System.Collections;
using App.SceneManagement;
using TGL.RPG.CommunicationBus;
using TGL.RPG.CommunicationBus.Sample;
using UnityEngine;

namespace TGL.RPG.Character
{


    public class OpenCharacterSelectionScreen : MonoBehaviour
    {
        private const int splashTime = 1;

        void Start()
        {
            StartCoroutine(EnterGame());
        }

        private static IEnumerator EnterGame()
        {
            yield return new WaitForSeconds(splashTime);
            MessageBus.PublishMessage(MessageTypes.ActivateSingleScene, new ChangeSceneEvent(AppSceneTypes.CharacterSelectionScene)); // change scene
        }
    }
}
