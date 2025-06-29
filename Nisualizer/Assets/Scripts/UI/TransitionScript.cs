using System.Collections;
using NnUtils.Modules.Easings;
using NnUtils.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI
{
    [RequireComponent(typeof(Image))]
    public class TransitionScript : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private bool _transition = true;
        [SerializeField] private float _transitionDelay = 1;
        [SerializeField] private float _transitionDuration = 2;
        [SerializeField] private Easings.Type _easing = Easings.Type.ExpoInOut;

        private void Reset() => _image = GetComponent<Image>();
        
        private void Awake()
        {
            if (_transition) return;
            Destroy(transform.parent.gameObject);
        }
        
        private void Start() => StartCoroutine(TransitionRoutine());

        private IEnumerator TransitionRoutine()
        {
            yield return new WaitForSeconds(_transitionDelay);
            
            float lerpPos = 0;
            var startCol = _image.color;
            var targetCol = _image.color;
            targetCol.a = 0;

            while (lerpPos < 1)
            {
                var t = Misc.Tween(ref lerpPos, _transitionDuration);
                _image.color = Vector4.Lerp(startCol, targetCol, t);
                yield return null;
            }

            Destroy(transform.parent.gameObject);
        }
    }
}