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
            var vfx = ConfigData.VFX;
            
            _chaoticCubesVFX.SetVector2("SpawnRateRange", vfx.SpawnRateRange);
            _chaoticCubesVFX.SetVector2("LifetimeRange", vfx.LifetimeRange);
            _chaoticCubesVFX.SetVector2("SpeedRange", vfx.SpeedRange);
            
            _chaoticCubesVFX.SetFloat("TurbulenceRotationSpeed", vfx.Turbulence.RotationSpeed);
            _chaoticCubesVFX.SetVector2("TurbulenceIntensityRange", vfx.Turbulence.IntensityRange);
            _chaoticCubesVFX.SetVector2("TurbulenceDragRange", vfx.Turbulence.DragRange);
            _chaoticCubesVFX.SetFloat("TurbulenceFrequency", vfx.Turbulence.Frequency);
            _chaoticCubesVFX.SetInt("TurbulenceOctaves", vfx.Turbulence.Octaves);
            _chaoticCubesVFX.SetFloat("TurbulenceRoughness", vfx.Turbulence.Roughness);
            _chaoticCubesVFX.SetFloat("TurbulenceLacunarity", vfx.Turbulence.Lacunarity);
            
            _chaoticCubesVFX.SetGradient("ColorOverLife", ConfigData.VFX.ColorOverLife);
        }
    }
}
