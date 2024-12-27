using TMPro;
using UnityEngine;

namespace InteractiveComponents.Text
{
    [RequireComponent(typeof(TMP_Text))]
    public class InteractiveTMPText : InteractiveTextScript
    {
        [SerializeField] private TMP_Text _tmpText;

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
            _tmpText.text = _text;
        }
    }
}