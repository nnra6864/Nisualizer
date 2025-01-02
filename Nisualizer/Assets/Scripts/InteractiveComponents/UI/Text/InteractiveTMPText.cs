using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace InteractiveComponents.UI.Text
{
    [RequireComponent(typeof(TMP_Text))]
    public class InteractiveTMPText : InteractiveTextScript
    {
        [SerializeField] private List<TMP_Text> _textElements;
        [SerializeField] private string _textE;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space)) UpdateData(new(){_textE});
        }

        private void Reset()
        {
            _textElements = GetComponents<TMP_Text>().ToList();
        }

        protected override void UpdateDefaultFont()
        {
            
        }

        protected override void UpdateFont()
        {
            //_textElements.font = _font;
        }

        protected override void SetText(string text, int index)
        {
            _textElements[index].text = text;
        }
    }
}