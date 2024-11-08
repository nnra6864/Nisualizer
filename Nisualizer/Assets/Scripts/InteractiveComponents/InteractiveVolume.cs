using Core;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace InteractiveComponents
{
    [RequireComponent(typeof(Volume))]
    public class InteractiveVolume : MonoBehaviour
    {
        [SerializeField] private MicrophoneDataScript _microphoneData;
        [SerializeField] private Volume _volume;
    
        private ColorAdjustments _colorAdjustments;
        [SerializeField] private Vector2 _postExpoRange = new(0, 0.5f);
        [SerializeField] private Vector2 _contrastRange = new(0, 10);
        [SerializeField] private Vector2 _saturationRange= new(0, 25);

        private Bloom _bloom;
        [SerializeField] private Vector2 _bloomIntensityRange = new(1, 2);
        [SerializeField] private Vector2 _bloomScatterRange = new(0.5f, 0.75f);
        
        private LensDistortion _lensDistortion;
        [SerializeField] private Vector2 _lensDistortionRange = new(0, -0.1f);

        private Vignette _vignette;
        [SerializeField] private Vector2 _vignetteRange = new(0.25f, 0.3f);
    
        private void Reset()
        {
            _microphoneData = FindFirstObjectByType<MicrophoneDataScript>();
            _volume         = GetComponent<Volume>();
        }

        private void Start()
        {
            _volume.profile.TryGet(out _colorAdjustments);
            _volume.profile.TryGet(out _bloom);
            _volume.profile.TryGet(out _lensDistortion);
            _volume.profile.TryGet(out _vignette);
        }

        private void Update()
        {
            //Color Adjustments
            _colorAdjustments.postExposure.value = Mathf.LerpUnclamped(_postExpoRange.x, _postExpoRange.y, _microphoneData.Loudness);
            _colorAdjustments.contrast.value     = Mathf.LerpUnclamped(_contrastRange.x, _contrastRange.y, _microphoneData.Loudness);
            _colorAdjustments.saturation.value   = Mathf.LerpUnclamped(_saturationRange.x, _saturationRange.y, _microphoneData.Loudness);

            //Bloom
            _bloom.intensity.value = Mathf.LerpUnclamped(_bloomIntensityRange.x, _bloomIntensityRange.y, _microphoneData.Loudness);
            _bloom.scatter.value   = Mathf.LerpUnclamped(_bloomScatterRange.x, _bloomScatterRange.y, _microphoneData.Loudness);

            //Lens Distortion
            _lensDistortion.intensity.value = Mathf.LerpUnclamped(_lensDistortionRange.x, _lensDistortionRange.y, _microphoneData.Loudness);

            //Vignette
            _vignette.intensity.value = Mathf.LerpUnclamped(_vignetteRange.x, _vignetteRange.y, _microphoneData.Loudness);
        }
    }
}