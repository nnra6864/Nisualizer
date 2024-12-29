using UnityEngine;
using UnityEngine.UIElements;

namespace InteractiveComponents.Text
{
    public class InteractiveLabel : InteractiveTextScript
    {
        [SerializeField] private VisualElement _rootElement;
        [SerializeField] private Label _label;

        private void Reset()
        {
            // Get the root VisualElement and Label component from the UI Document
            _rootElement = GetComponent<UIDocument>().rootVisualElement;
            _label       = _rootElement.Q<Label>();
        }
        
        //protected override void UpdateDefaultFont() => _defaultFont = _label.style.unityFontDefinition.value.fontAsset = //The font asset;

        //protected override void UpdateFont()
        //{
        //    base.UpdateFont();
        //    _tmpText.font = _font;
        //}

        protected override void SetText(string text)
        {
            _label.text = _text;
        }
    }
}