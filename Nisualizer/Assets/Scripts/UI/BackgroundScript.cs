using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Image))]
    public class BackgroundScript : MonoBehaviour
    {
        [SerializeField] private Image _image;
        
        private void Reset() => _image = GetComponent<Image>();

        private void Start() => _image.sprite = GameManager.Background;
    }
}