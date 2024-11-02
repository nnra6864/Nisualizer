using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Volume))]
public class InteractiveVolume : MonoBehaviour
{
    [SerializeField] private MicrophoneDataScript _microphoneData;
    [SerializeField] private Volume _volume;
    
    private LensDistortion _lensDistortion;
    [SerializeField] private Vector2 _lensDistortionRange = new(0, -0.1f);

    private Vignette _vignette;
    [SerializeField] private Vector2 _vignetteRange = new(0.25f, 0.3f);
    
    private void Reset()
    {
        _microphoneData = FindFirstObjectByType<MicrophoneDataScript>();
        _volume = GetComponent<Volume>();
    }

    private void Start()
    {
        _volume.profile.TryGet(out _lensDistortion);
        _volume.profile.TryGet(out _vignette);
    }

    private void Update()
    {
        _lensDistortion.intensity.value = Mathf.LerpUnclamped(_lensDistortionRange.x, _lensDistortionRange.y, _microphoneData.Loudness);
        _vignette.intensity.value       = Mathf.LerpUnclamped(_vignetteRange.x, _vignetteRange.y, _microphoneData.Loudness);
    }
}