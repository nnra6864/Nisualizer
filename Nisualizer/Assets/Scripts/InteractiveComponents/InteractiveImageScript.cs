using System.Collections.Generic;
using NnUtils.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace InteractiveComponents
{
    public class InteractiveImageScript : MonoBehaviour
    {
        private readonly List<Image> _images = new();
        
        private AspectRatioFitter _systemImageARF;
        [SerializeField] private Image _systemImage;

        private void Awake()
        {
            _systemImageARF = _systemImage.GetComponent<AspectRatioFitter>();
            _images.AddRange(GetComponentsInChildren<Image>(true));
        }

        public void LoadImage(string image)
        {
            // Disable all the images
            DisableAll();
            
            // Return if image string is empty
            if (string.IsNullOrEmpty(image)) return;
            
            // Try to load the image from a path
            var systemSprite = Misc.SpriteFromFile(image);
            if (systemSprite != null)
            {
                // Load the image and store rect
                _systemImage.sprite = systemSprite;
                var rect = systemSprite.rect;
                
                // Set aspect ratio
                _systemImageARF.aspectRatio = rect.width / rect.height;
                
                // Enable the object and return
                _systemImage.gameObject.SetActive(true);
                return;
            }
            
            // Find the image by name and enable it
            _images.Find(x => x.name == image)?.gameObject.SetActive(true);
        }

        private void DisableAll()
        {
            foreach (var image in _images)
                image.gameObject.SetActive(false);
        }
    }
}