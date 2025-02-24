using Scripts.Core;
using UnityEngine;

namespace Scripts.InteractiveComponents
{
    [RequireComponent(typeof(ParticleSystem))]
    public class InteractiveParticleSystemScript : MonoBehaviour
    {
        private static AudioDataScript AudioData => GameManagerScript.AudioData;
        
        [SerializeField] private ParticleSystem _particleSystem;
        [SerializeField] private Vector2 _simSpeedRange = new(0.1f, 1);

        private void Reset()
        {
            _particleSystem = GetComponent<ParticleSystem>();
        }

        private void Update()
        {
            var main = _particleSystem.main;
            main.simulationSpeed = Mathf.LerpUnclamped(_simSpeedRange.x, _simSpeedRange.y, AudioData.Loudness);
        }
    }
}