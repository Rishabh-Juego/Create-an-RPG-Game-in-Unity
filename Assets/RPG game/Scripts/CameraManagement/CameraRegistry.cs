using UnityEngine;

namespace TGL.RPG.CameraManagement
{
    public class CameraRegistry
    {
        private static IActiveCameraProvider _currentProvider;

        public static void Register(IActiveCameraProvider provider) => _currentProvider = provider;

        public static void Unregister(IActiveCameraProvider provider)
        {
            if (_currentProvider == provider)
            {
                _currentProvider = null;
            }
        }
        
        public static IActiveCameraProvider Get() => _currentProvider;
    }
}