using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Config;
using Core;
using NnUtils.Scripts;
using TMPro;
using UnityEngine;

namespace InteractiveComponents.UI.Text
{
    public class InteractiveTextScript : MonoBehaviour
    {
        // General Config Data
        private static GeneralConfigData _configData;
        private static GeneralConfigData ConfigData => _configData ??= (GeneralConfigData)GameManagerScript.ConfigScript.Data;

        /// Font applied to text in editor
        protected TMP_FontAsset _defaultFont;

        /// Font found in the config
        protected TMP_FontAsset _font;

        /// Stores the unprocessed text
        protected string _text;

        /// Stores dynamic text values
        private List<DynamicText> _dynamicText;

        private void Start()
        {
            UpdateDefaultFont();
            OnConfigLoaded();
            ConfigData.OnLoaded += OnConfigLoaded;
        }

        private void OnDestroy()
        {
            ConfigData.OnLoaded -= OnConfigLoaded;
        }

        /// Gets called on <see cref="GeneralConfigData.OnLoaded"/>
        private void OnConfigLoaded() => UpdateData(_text);

        /// Updates all the data
        public void UpdateData(string text)
        {
            UpdateFont();
            UpdateText(text);
        }

        /// Updates the default font by taking the current one, should be called in <see cref="Start"/>
        protected virtual void UpdateDefaultFont() { }

        protected virtual void UpdateFont()
        {
            var font = ConfigData.Font;
            _font = font == "Default" ? _defaultFont : SystemTMP.GenerateFontFromName(font);
        }

        /// Handles text being changed
        private void UpdateText(string text)
        {
            // Return if text hasn't changed
            if (_text == text) return;
            _text = text;

            // Get a list of dynamic texts found in text
            _dynamicText = InteractiveTextProcessing.GetDynamicText(_text);

            // Stop all the running dynamic text routines
            foreach (var routine in _updateDynamicTextRoutines) StopCoroutine(routine);

            // Start a routine for each dynamic text
            for (var i = 0; i < _dynamicText.Count; i++)
            {
                // If interval is null, assign the value once and return
                if (_dynamicText[i].Interval == null)
                {
                    // TODO: Maybe make async like in routine
                    _dynamicText[i].Text = _dynamicText[i].Func();
                    SetText(InteractiveTextProcessing.ReplaceWithDynamicText(_text, new(_dynamicText)));
                    continue;
                }

                // Start the dynamic text routine and add it to the list
                _updateDynamicTextRoutines.Add(StartCoroutine(UpdateDynamicTextRoutine(i)));
            }
        }

        /// Stores all the update dynamic text routines
        private readonly List<Coroutine> _updateDynamicTextRoutines = new();

        /// Updates an instance of dynamic text and triggers the text update
        private IEnumerator UpdateDynamicTextRoutine(int index)
        {
            // Store dynamic text
            var dt = _dynamicText[index];

            while (true)
            {
                // Update the dynamic text value async
                var task = Task.Run(() => dt.Func());
                yield return new WaitUntil(() => task.IsCompleted);
                dt.Text = task.Result;

                // Update the text
                SetText(InteractiveTextProcessing.ReplaceWithDynamicText(_text, new(_dynamicText)));

                // Wait for the value of Interval
                yield return new WaitForSecondsRealtime((float)dt.Interval!);
            }
        }

        /// Sets the text of the ui component
        protected virtual void SetText(string text) { }
    }
}