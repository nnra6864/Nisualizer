using Core;
using Scenes.ChaoticCubes.Config;
using UnityEngine;
using UnityEngine.VFX;

namespace Scenes.ChaoticCubes.Scripts
{
    public class ChaoticCubesManager : SceneManagerScript
    {
        public static ChaoticCubesConfigData ConfigData => (ChaoticCubesConfigData)Config.Data;

        [SerializeField] private VisualEffect _chaoticCubesVFX;
        
        private void Start()
        {
            ConfigData.OnLoaded += OnConfigLoaded;
        }

        private void OnDestroy()
        {
            ConfigData.OnLoaded -= OnConfigLoaded;
        }

        private void OnConfigLoaded()
        {
            
        }

        private void UpdateChaoticCubesVFX()
        {
            _chaoticCubesVFX.SetGradient("ColorOverLife", ConfigData.CubeColorOverLife);
        }
    }
}
