using System.Collections;
using System.Linq;
using Config;
using NnUtils.Scripts;
using UnityEngine;

namespace Core
{
    public class MicrophoneDataScript : MonoBehaviour
    {
        private static GeneralConfigData _configData;
        private static GeneralConfigData ConfigData => _configData ??= (GeneralConfigData)GameManagerScript.ConfigScript.Data;
        
        //Higher values lead to a smoother appearance but also add more delay
        private const int SampleWindow = 64;
    
        //Name of the input device that's being used
        private static string InputName => ConfigData.InputName;
    
        //Stores the AudioClip used in loudness detection
        private AudioClip _microphoneClip;

        //Represents the current audio loudness
        public float Loudness { get; private set; }
    
        [Tooltip("Time in seconds it will take to transition to the new Loudness value.")]
        [SerializeField] private float _transitionTime = 0.25f;
        [Tooltip("Easing applied to the Loudness transition.")]
        [SerializeField] private Easings.Type _transitionEasing = Easings.Type.CubicOut;

        /// <summary>
        /// Called after initializing config in the <see cref="GameManagerScript"/><br/>
        /// Should also be called on <see cref="Config.ConfigData.OnLoaded"/>
        /// </summary>
        public void InitializeMicrophone()
        {
            if (string.IsNullOrEmpty(InputName) || !Microphone.devices.Contains(InputName))
            {
                Debug.LogWarning($"Input named {InputName} doesn't exist, returning");
                return;
            }
            _microphoneClip = Microphone.Start(InputName, true, 20, AudioSettings.outputSampleRate);
        }

        private void Start()
        {
            ConfigData.OnLoaded += InitializeMicrophone;
        }

        private void Update()
        {
            TweenMicrophoneLoudness(GetLoudness() * ConfigData.Sensitivity);
        }

        private void OnDestroy()
        {
            ConfigData.OnLoaded -= InitializeMicrophone;
        }

        //Returns the total loudness of microphone audio
        private float GetLoudness()
        {
            //Get microphone clip position and return 0 if negative
            var pos = Microphone.GetPosition(InputName) - SampleWindow;
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