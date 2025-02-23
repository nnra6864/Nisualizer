using System.Collections;
using System.Linq;
using NnUtils.Modules.SystemAudioMonitor;
using NnUtils.Modules.SystemAudioMonitor.CoreAudio;
using NnUtils.Scripts;
using Scripts.Config;
using Scripts.Core;
using UnityEngine;

namespace Scripts.Audio
{
    public class AudioDataScript : MonoBehaviour
    {
        private static GeneralConfigData _configData;
        private static GeneralConfigData ConfigData => _configData ??= (GeneralConfigData)GameManagerScript.ConfigScript.Data;
        
        //Higher values lead to a smoother appearance but also add more delay
        private int _bufferSize = 64;
    
        // TODO: Implement specific device listening
        //Name of the input device that's being used
        private static string AudioDevice => ConfigData.AudioDevice;
    
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
        public void Initialize()
        {
            _bufferSize = ConfigData.BufferSize;
            AudioMonitorManager.Start(AudioCaptureType.Default, "Nisualizer", bufferSize: _bufferSize, device: ConfigData.AudioDevice);
        }

        private void Start()
        {
            ConfigData.OnLoaded += Initialize;
        }

        private void Update()
        {
            TweenMicrophoneLoudness(AudioMonitorManager.Loudness * ConfigData.Sensitivity);
        }

        private void OnDestroy()
        {
            ConfigData.OnLoaded -= Initialize;
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