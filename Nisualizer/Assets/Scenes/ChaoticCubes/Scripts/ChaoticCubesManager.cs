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
            UpdateChaoticCubesVFX();
            ConfigData.OnLoaded += OnConfigLoaded;
        }

        private void OnDestroy()
        {
            ConfigData.OnLoaded -= OnConfigLoaded;
        }

        private void OnConfigLoaded()
        {
            UpdateChaoticCubesVFX();
        }

        private void UpdateChaoticCubesVFX()
        {
            _chaoticCubesVFX.SetVector2("SpawnRateRange", ConfigData.VFX.SpawnRateRange);
            _chaoticCubesVFX.SetGradient("ColorOverLife", ConfigData.VFX.ColorOverLife);
        }
    }
}
