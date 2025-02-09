using NnUtils.Modules.JSONUtils.Scripts.Types.Components.UI.Image;
using Scenes.ChaoticCubes.Config;
using Scripts.Core;
using Scripts.InteractiveComponents;
using UnityEngine;
using UnityEngine.VFX;

namespace Scenes.ChaoticCubes.Scripts
{
    public class ChaoticCubesManager : SceneManagerScript
    {
        public static ChaoticCubesConfigData ConfigData => (ChaoticCubesConfigData)Config.Data;

        private GameObject _objects;

        [SerializeField] private InteractiveImageScript _background;
        [SerializeField] private Camera _camera;
        [SerializeField] private Light _directionalLight;
        [SerializeField] private VisualEffect _chaoticCubesVFX;
        
        private void Start()
        {
            _objects = new("Objects");
            OnConfigLoaded();
            ConfigData.OnLoaded += OnConfigLoaded;
        }

        private void OnDestroy()
        {
            ConfigData.OnLoaded -= OnConfigLoaded;
        }

        private void OnConfigLoaded()
        {
            //UpdateBackground();
            UpdateCamera();
            UpdateLight();
            UpdateChaoticCubesVFX();
            UpdateObjects();
        }

        //private void UpdateBackground() => _background.LoadImage(ConfigData.Background);
        
        private void UpdateCamera()
        {
            var camPos = _camera.transform.localPosition;
            _camera.transform.localPosition = new(camPos.x, camPos.y, -ConfigData.CameraDistance);
            _camera.nearClipPlane = ConfigData.CameraClippingPlanes.X;
            _camera.farClipPlane  = ConfigData.CameraClippingPlanes.Y;
        }

        private void UpdateLight() => _directionalLight.intensity = ConfigData.DirectionalLightIntensity;
        
        private void UpdateChaoticCubesVFX()
        {
            // Store VFX
            var vfx = ConfigData.VFX;
            
            // Update general properties
            _chaoticCubesVFX.SetVector2("SpawnRateRange", vfx.SpawnRateRange);
            _chaoticCubesVFX.SetFloat("SpawnRadius", vfx.SpawnRadius);
            _chaoticCubesVFX.SetVector2("LifetimeRange", vfx.LifetimeRange);
            _chaoticCubesVFX.SetVector2("SpeedRange", vfx.SpeedRange);
            
            // Update turbulence properties
            _chaoticCubesVFX.SetFloat("TurbulenceRotationSpeed", vfx.Turbulence.RotationSpeed);
            _chaoticCubesVFX.SetVector2("TurbulenceIntensityRange", vfx.Turbulence.IntensityRange);
            _chaoticCubesVFX.SetVector2("TurbulenceDragRange", vfx.Turbulence.DragRange);
            _chaoticCubesVFX.SetFloat("TurbulenceFrequency", vfx.Turbulence.Frequency);
            _chaoticCubesVFX.SetInt("TurbulenceOctaves", vfx.Turbulence.Octaves);
            _chaoticCubesVFX.SetFloat("TurbulenceRoughness", vfx.Turbulence.Roughness);
            _chaoticCubesVFX.SetFloat("TurbulenceLacunarity", vfx.Turbulence.Lacunarity);
            
            // Update mesh properties
            _chaoticCubesVFX.SetFloat("MeshSmoothness", vfx.MeshSmoothness);
            _chaoticCubesVFX.SetFloat("MeshMetallic", vfx.MeshMetallic);
            _chaoticCubesVFX.SetFloat("MeshSize", vfx.MeshSize);
            _chaoticCubesVFX.SetGradient("ColorOverLife", ConfigData.VFX.ColorOverLife);
        }
        
        private void UpdateObjects()
        {
            // Destroy all existing objects
            foreach (Transform child in _objects.transform) Destroy(child.gameObject);
            
            // Add new objects
            foreach (var configDataObject in ConfigData.Objects)
            {
                GameObject child = new();
                child.transform.SetParent(_objects.transform);
                configDataObject.Initialize(child);
            }
        }
    }
}
