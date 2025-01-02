using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace InteractiveComponents.UI.Text
{
    public class InteractiveLabel : InteractiveTextScript
    {
        [SerializeField] private VisualElement _rootElement;
        [SerializeField] private List<Label> _labels;

        private void Reset()
        {
            // Get the root VisualElement and Label components from the UI Document
            _rootElement = GetComponent<UIDocument>().rootVisualElement;
            _labels      = _rootElement.Query<Label>().ToList();
        }
        
        //protected override void UpdateDefaultFont() => _defaultFont = _label.style.unityFontDefinition.value.fontAsset = //The font asset;

        protected override void UpdateFont()
        {
            base.UpdateFont();
            //_tmpText.font = _font;
        }

        protected override void SetText(string text)
        {
            //_label.text = _text;
        }
    }
}