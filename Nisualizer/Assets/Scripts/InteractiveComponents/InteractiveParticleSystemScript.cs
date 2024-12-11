using Core;
using UnityEngine;

namespace InteractiveComponents
{
    [RequireComponent(typeof(ParticleSystem))]
    public class InteractiveParticleSystemScript : MonoBehaviour
    {
        private static MicrophoneDataScript MicrophoneData => GameManagerScript.MicrophoneData;
        
        [SerializeField] private ParticleSystem _particleSystem;
        [SerializeField] private Vector2 _simSpeedRange = new(0.1f, 1);

        private void Reset()
        {
            _particleSystem = GetComponent<ParticleSystem>();
        }

        private void Update()
        {
            var main = _particleSystem.main;
            main.simulationSpeed = Mathf.LerpUnclamped(_simSpeedRange.x, _simSpeedRange.y, MicrophoneData.Loudness);
        }
    }
}