using System.Collections;
using System.Collections.Generic;
using System.Threading;
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
        private List<string> _text = new();

        /// Stores dynamic text values <br/>
        /// Key represents the index of the text element that dynamic text is made from <br/>
        /// Value represents the list of all the dynamic text instances found in that text element
        private readonly Dictionary<int, List<DynamicText>> _dynamicText = new();

        protected virtual void Start()
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
        public void OnConfigLoaded() => UpdateData(_text);

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
            // Assign text elements
            _text = text;

            // Clear and fill the _dynamicText dictionary
            _dynamicText.Clear();
            for (var i = 0; i < _text.Count; i++)
                _dynamicText.Add(i, InteractiveTextProcessing.GetDynamicText(_text[i]));

            // Stop all the running dynamic text routines
            foreach (var routine in _updateDynamicTextRoutines) StopCoroutine(routine);

            // Update text
            for (var i = 0; i < _text.Count; i++)
            {
                // If there is no dynamic text, just set the text
                if (_dynamicText[i].Count == 0)
                {
                    SetText(_text[i], i);
                    continue;
                }

                // Update all dynamic text instances
                for (var o = 0; o < _dynamicText[i].Count; o++)
                    _updateDynamicTextRoutines.Add(StartCoroutine(UpdateDynamicTextRoutine(i, o)));
            }
        }

        /// Stores all the update dynamic text routines
        private readonly List<Coroutine> _updateDynamicTextRoutines = new();

        /// Updates an instance of dynamic text and triggers the text update
        private IEnumerator UpdateDynamicTextRoutine(int i, int o)
        {
            var dt = _dynamicText[i][o];
        
            while (true)
            {
                // Store the start time
                var startTime = Time.realtimeSinceStartup;
        
                // Update the dynamic text value async
                var task = Task.Run(() => dt.Func());
                yield return new WaitUntil(() => task.IsCompleted);
        
                // If task finished running, run SetText()
                if (task.Status == TaskStatus.RanToCompletion)
                {
                    dt.Text = task.Result;
                    SetText(InteractiveTextProcessing.ReplaceWithDynamicText(_text[i], new(_dynamicText[i])), i);
                }
        
                // Get interval and break if null
                var interval = dt.Interval;
                if (interval == null) yield break;
        
                // Calculate and wait for remaining time
                var elapsedTime = Time.realtimeSinceStartup - startTime;
                var remainingTime = Mathf.Max(0, (float)interval - elapsedTime);
                if (remainingTime > 0) yield return new WaitForSecondsRealtime(remainingTime);
            }
        }
        
        /// Sets the text of the ui component
        protected virtual void SetText(string text, int index) { }
    }
}