using System;
using TGL.RPG.CommunicationBus;
using TGL.RPG.CommunicationBus.Sample;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TGL.RPG.SceneHandling
{


    public class SceneHandler : MonoBehaviour
    {
        private void Awake()
        {
            RegisterMessageBusEvents();
            DontDestroyOnLoad(gameObject);
        }

        private void OnDestroy()
        {
            UnregisterMessageBusEvents();
        }

        private void RegisterMessageBusEvents()
        {
            MessageBus.RegisterMessageListener(MessageTypes.ActivateSingleScene, ChangeActiveScene);
        }

        private void UnregisterMessageBusEvents()
        {
            MessageBus.UnregisterMessageListener(MessageTypes.ActivateSingleScene, ChangeActiveScene);
        }

        private void ChangeActiveScene(IMessageBody msgBody)
        {
            if (msgBody is not ChangeSceneEvent changeSceneEvent) return;
            int buildIndex = (int)changeSceneEvent.sceneType;

            Debug.Log($"Trying to change to scene type {changeSceneEvent.sceneType} with build index {buildIndex}/{SceneManager.sceneCountInBuildSettings}");
            if (SceneManager.sceneCountInBuildSettings <= buildIndex)
            {
                throw new ArgumentOutOfRangeException(nameof(changeSceneEvent.sceneType),
                    changeSceneEvent.sceneType, $"No entry for scene type {changeSceneEvent.sceneType} found");
            }

            SceneManager.LoadScene(buildIndex, LoadSceneMode.Single);
        }
    }
}
