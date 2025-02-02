using Scripts.Audio;
using Scripts.Core;
using UnityEngine;
using UnityEngine.Video;

namespace Scripts.InteractiveComponents
{
    [RequireComponent(typeof(VideoPlayer))]
    public class InteractiveVideoPlayerScript : MonoBehaviour
    {
        private static AudioDataScript AudioData => GameManagerScript.AudioData;

        [SerializeField] private VideoPlayer _videoPlayer;
        
        [Tooltip("Set to 1, 1 for no effect")]
        [SerializeField] private Vector2 _playbackSpeedRange = new(1, 2);
        
        private void Reset()
        {
            _videoPlayer = GetComponent<VideoPlayer>();
        }

        private void Update()
        {
            _videoPlayer.playbackSpeed = Mathf.LerpUnclamped(_playbackSpeedRange.x, _playbackSpeedRange.y, AudioData.Loudness);
        }
    }
}