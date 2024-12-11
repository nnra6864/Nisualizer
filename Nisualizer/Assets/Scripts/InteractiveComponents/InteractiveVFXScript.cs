using Core;
using UnityEngine;
using UnityEngine.VFX;

namespace InteractiveComponents
{
    [RequireComponent(typeof(VisualEffect))]
    public class InteractiveVFXScript : MonoBehaviour
    {
        private static MicrophoneDataScript MicrophoneDataScript => GameManagerScript.MicrophoneDataScript;
        
        [SerializeField] private VisualEffect _vfx;
    
        [Tooltip("Name of the parameter in your VFX graph that'll be assigned a value of microphone loudness")]
        [SerializeField] private string _loudnessPropertyName = "Loudness";
    
        [Tooltip("Set to 1, 1 for no effect")]
        [SerializeField] private Vector2 _playRateRange = new(1, 1);

        private void Reset()
        {
            _vfx = GetComponent<VisualEffect>();
        }

        private void Update()
        {
            _vfx.playRate = Mathf.LerpUnclamped(_playRateRange.x, _playRateRange.y, MicrophoneDataScript.Loudness);
            _vfx.SetFloat(_loudnessPropertyName, MicrophoneDataScript.Loudness);
        }
    }
}