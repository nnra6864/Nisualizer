using System;
using TMPro;
using UnityEngine;

namespace InteractiveComponents.Text
{
    [RequireComponent(typeof(TMP_Text))]
    public class InteractiveTMPText : InteractiveTextScript
    {
        [SerializeField] private TMP_Text _tmpText;
        [SerializeField] private string _textE;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space)) UpdateData(_textE);
        }

        private void Reset()
        {
            _tmpText = GetComponent<TMP_Text>();
        }

        protected override void UpdateDefaultFont() => _defaultFont = _tmpText.font;

        protected override void UpdateFont()
        {
            base.UpdateFont();
            _tmpText.font = _font;
        }

        protected override void SetText(string text)
        {
            _tmpText.text = text;
        }
    }
}