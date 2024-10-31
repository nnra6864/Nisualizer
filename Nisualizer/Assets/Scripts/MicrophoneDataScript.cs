using System.Collections;
using NnUtils.Scripts;
using UnityEngine;

public class MicrophoneDataScript : MonoBehaviour
{
    private const int SampleWindow = 64;
    private readonly string _microphone = "OutputInput";
    private AudioClip _microphoneClip;

    public float Loudness { get; private set; }
    [SerializeField] private float _mutliplier = 0.25f;
    [SerializeField] private float _lerpTime = 0.25f;
    [SerializeField] private Easings.Type _easing = Easings.Type.CubicOut;
    
    private void Start() => InitializeMicrophone();

    private void InitializeMicrophone() =>
        _microphoneClip = Microphone.Start(_microphone, true, 20, AudioSettings.outputSampleRate);

    private float GetLoudness()
    {
        //Get microphone clip position and return 0 if negative
        var pos = Microphone.GetPosition(_microphone) - SampleWindow;
        if (pos < 0) return 0;
        
        //Get wave data from the microphone clip
        var waveData = new float[SampleWindow];
        _microphoneClip.GetData(waveData, pos);
        
        //Calculate loudness
        float loudness = 0;
        for (int i = 0; i < SampleWindow; i++)
            loudness += Mathf.Abs(waveData[i]);

        return loudness;
    }

    private void LerpMicrophoneLoudness(float loudness)
    {
        if (Mathf.Approximately(Loudness, loudness)) return;
        this.RestartRoutine(ref _lerpLoudnessRoutine, LerpLoudnessRoutine(loudness));
    }

    private Coroutine _lerpLoudnessRoutine;
    private IEnumerator LerpLoudnessRoutine(float loudness)
    {
        var startLoudness = Loudness;

        float lerpPos = 0;
        while (lerpPos < 1)
        {
            var t = Misc.Tween(ref lerpPos, _lerpTime, _easing);
            Loudness = Mathf.Lerp(startLoudness, loudness, t);
            yield return null;
        }
        
        _lerpLoudnessRoutine = null;
    }

    private void Update()
    {
        LerpMicrophoneLoudness(GetLoudness() * _mutliplier);
    }
}