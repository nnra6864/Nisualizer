using Core;
using UnityEngine;

namespace InteractiveComponents
{
    [RequireComponent(typeof(ParticleSystem))]
    public class InteractiveParticleSystemScript : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _particleSystem;
        [SerializeField] private MicrophoneDataScript _microphoneData;
        [SerializeField] private Vector2 _simSpeedRange = new(0.1f, 1);

        private void Reset()
        {
            _particleSystem = GetComponent<ParticleSystem>();
            _microphoneData = FindFirstObjectByType<MicrophoneDataScript>();
        }

        private void Update()
        {
            var main = _particleSystem.main;
            main.simulationSpeed = Mathf.LerpUnclamped(_simSpeedRange.x, _simSpeedRange.y, _microphoneData.Loudness);
        }
    }
}