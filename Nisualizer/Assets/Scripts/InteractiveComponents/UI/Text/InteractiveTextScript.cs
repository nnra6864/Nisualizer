using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Config;
using Core;
using UnityEngine;

namespace InteractiveComponents.UI.Text
{
    public class InteractiveTextScript : MonoBehaviour
    {
        // General Config Data
        private static GeneralConfigData _configData;
        protected static GeneralConfigData ConfigData => _configData ??= (GeneralConfigData)GameManagerScript.ConfigScript.Data;

        /// Stores the unprocessed text
        private List<string> _text;

        /// Stores dynamic text values
        private readonly Dictionary<int, List<DynamicText>> _dynamicText = new();

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
        public void UpdateData(List<string> text)
        {
            UpdateFont();
            UpdateText(text);
        }

        /// Updates the default font by taking the current one, should be called in <see cref="Start"/>
        protected virtual void UpdateDefaultFont()
        {
            throw new System.NotImplementedException("Derived class must implement UpdateDefaultFont.");
        }

        /// Updates the font for all text elements
        protected virtual void UpdateFont()
        {
            throw new System.NotImplementedException("Derived class must implement UpdateFont.");
        }

        /// Handles text being changed
        private void UpdateText(List<string> text)
        {
            // Return if text hasn't changed
            if (_text == text) return;
            _text = text;

            // Clear and fill the _dynamicText dictionary
            _dynamicText.Clear();
            for (var i = 0; i < _text.Count; i++)
                _dynamicText.Add(i, InteractiveTextProcessing.GetDynamicText(_text[i]));

            // Stop all the running dynamic text routines
            foreach (var routine in _updateDynamicTextRoutines) StopCoroutine(routine);

            // Update all the dynamic text instances
            for (var i = 0; i < _text.Count; i++)
                for (var o = 0; o < _dynamicText[i].Count; o++)
                    UpdateDynamicText(i, o);
        }
        
        /// Starts the update process for a specific DynamicText instance
        private void UpdateDynamicText(int i, int o)
        {
            // If there is no dynamic text, set the text and return
            if (_dynamicText[i].Count == 0)
            {
                SetText(_text[i], i);
                return;
            }

            // Start the dynamic text routine and add it to the list
            _updateDynamicTextRoutines.Add(StartCoroutine(UpdateDynamicTextRoutine(i, o)));
        }

        /// Stores all the update dynamic text routines
        private readonly List<Coroutine> _updateDynamicTextRoutines = new();

        /// Updates an instance of dynamic text and triggers the text update
        private IEnumerator UpdateDynamicTextRoutine(int i, int o)
        {
            // Store dynamic text
            var dt = _dynamicText[i][o];

            while (true)
            {
                // Update the dynamic text value async
                var task = Task.Run(() => dt.Func());
                yield return new WaitUntil(() => task.IsCompleted);
                dt.Text = task.Result;

                // Update the text
                SetText(InteractiveTextProcessing.ReplaceWithDynamicText(_text[i], new(_dynamicText[i])), i);

                // Wait for the value of Interval or break if null
                var interval = dt.Interval;
                if (interval == null) yield break;
                yield return new WaitForSecondsRealtime((float)interval!);
            }
        }

        /// Sets the text of the ui component
        protected virtual void SetText(string text, int index) { }
    }
}