using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace InteractiveComponents.UI.Text
{
    [RequireComponent(typeof(UIDocument))]
    public class InteractiveLabel : InteractiveTextScript
    {
        private VisualElement _rootElement;
        private List<Label> _labels;

        private void Awake()
        {
            // Get the root VisualElement and Label components from the UI Document
            _rootElement = GetComponent<UIDocument>().rootVisualElement;

            // Return if no root was found
            if (_rootElement == null) return;
            
            // Get all the labels
            _labels      = _rootElement.Query<Label>().ToList();
        }

        protected override void Start()
        {
            base.Start();
            
            // Update all the label elements text
            UpdateData(_labels.Select(x => x.text).ToList());
        }
        
        protected override void UpdateDefaultFont()
        {
            
        }

        protected override void UpdateFont()
        {
            
        }

        protected override void SetText(string text, int index)
        {
            _labels[index].text = text;
        }
    }
}