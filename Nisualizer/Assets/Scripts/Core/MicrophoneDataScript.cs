using System.Collections;
using Config;
using NnUtils.Scripts;
using PortAudioSharp;
using UnityEngine;

namespace Core
{
    public class MicrophoneDataScript : MonoBehaviour
    {
        private static GeneralConfigData ConfigData => (GeneralConfigData)GameManager.ConfigScript.Data;
        
        //More values lead to a smoother appearance but also add more delay
        private const int SampleWindow = 64;
    
        //Name of the input device that's being used
        //TODO: Implement custom names in config for _microphone
        private string _microphone = "OutputInput";
        //private string _microphone = "CABLE Output (VB-Audio Virtual Cable)";
    
        //Stores the AudioClip used in loudness detection
        private AudioClip _microphoneClip;

        //Represents the current audio loudness
        public float Loudness { get; private set; }
    
        [Tooltip("Time in seconds it will take to transition to the new Loudness value.")]
        [SerializeField] private float _transitionTime = 0.25f;
        [Tooltip("Easing applied to the Loudness transition.")]
        [SerializeField] private Easings.Type _transitionEasing = Easings.Type.CubicOut;

        /// <summary>
        /// This function should be called from the GameManager after initializing ConfigScript
        /// </summary>
        public void InitializeMicrophone() =>
            _microphoneClip = Microphone.Start(_microphone, true, 20, AudioSettings.outputSampleRate);

        private void Update()
        {
            TweenMicrophoneLoudness(GetLoudness() * ConfigData.Sensitivity);
        }

        private void StartPortAudio()
        {
            PortAudio.Initialize();
        }
        
        //Returns the total loudness of microphone audio
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

        //Tweens the loudness for a smoother look
        private void TweenMicrophoneLoudness(float loudness)
        {
            //If new loudness is approximately the same as current, return, otherwise tween
            if (Mathf.Approximately(Loudness, loudness)) return;
            this.RestartRoutine(ref _tweenLoudnessRoutine, TweenLoudnessRoutine(loudness));
        }

        private Coroutine _tweenLoudnessRoutine;
        private IEnumerator TweenLoudnessRoutine(float loudness)
        {
            //Store the current loudness
            var startLoudness = Loudness;

            //Start the tweening loop
            float lerpPos = 0;
            while (lerpPos < 1)
            {
                //Update the lerp position and store the eased value
                var t = Misc.Tween(ref lerpPos, _transitionTime, _transitionEasing);
            
                //Update the loudness
                Loudness = Mathf.Lerp(startLoudness, loudness, t);
            
                //Wait for the next frame
                yield return null;
            }
        
            //Set the coroutine value null to avoid an unnecessary call
            _tweenLoudnessRoutine = null;
        }
    }
}