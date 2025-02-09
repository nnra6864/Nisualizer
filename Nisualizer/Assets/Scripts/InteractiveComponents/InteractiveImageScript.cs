using System.Collections.Generic;
using NnUtils.Modules.JSONUtils.Scripts.Types.Components.UI;
using NnUtils.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.InteractiveComponents
{
    public class InteractiveImageScript : MonoBehaviour
    {
        private List<Image> _images = new();
        private AspectRatioFitter _systemImageARF;
        private ConfigImage _configImage;

        private void Awake()
        {
            //_systemImageARF = _systemImage.GetComponent<AspectRatioFitter>();
            _images.AddRange(GetComponentsInChildren<Image>(true));
        }

        public void LoadData(ConfigImage img)
        {
            if (_configImage != null && img != null && img.Equals(_configImage)) return;
            _configImage = img;
            // TODO: Implement image loading and transitioning
        }

        public void LoadImage(string image)
        {
            // Return if image string is empty
            if (string.IsNullOrEmpty(image)) return;
            
            // Try to load the image from a path
            var systemSprite = Misc.SpriteFromFile(image);
            if (systemSprite != null)
            {
                // Load the image and store rect
                //_systemImage.sprite = systemSprite;
                var rect = systemSprite.rect;
                
                // Set aspect ratio
                _systemImageARF.aspectRatio = rect.width / rect.height;
                
                // Enable the object and return
                //_systemImage.gameObject.SetActive(true);
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