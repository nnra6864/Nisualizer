using Core;
using UnityEngine;
using UnityEngine.Video;

namespace InteractiveComponents
{
    [RequireComponent(typeof(VideoPlayer))]
    public class InteractiveVideoPlayerScript : MonoBehaviour
    {
        private static MicrophoneDataScript MicrophoneData => GameManagerScript.MicrophoneData;

        [SerializeField] private VideoPlayer _videoPlayer;
        
        [Tooltip("Set to 1, 1 for no effect")]
        [SerializeField] private Vector2 _playbackSpeedRange = new(1, 1);
        
        private void Reset()
        {
            _videoPlayer = GetComponent<VideoPlayer>();
        }

        private void Update()
        {
            _videoPlayer.playbackSpeed = Mathf.LerpUnclamped(_playbackSpeedRange.x, _playbackSpeedRange.y, MicrophoneData.Loudness);
        }
    }
}