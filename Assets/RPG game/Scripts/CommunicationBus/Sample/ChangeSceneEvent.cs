using App.SceneManagement;
using TGL.RPG.CommunicationBus;
using TGL.RPG.GameCursor;

namespace RPG_Game.Scripts.CommunicationBus.Sample
{
    public class ChangeSceneEvent : IMessageBody
    {
        public AppSceneTypes sceneType;

        public ChangeSceneEvent(AppSceneTypes nextSceneType)
        {
            sceneType = nextSceneType;
        }
    }
}