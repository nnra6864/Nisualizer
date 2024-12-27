using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Config;
using Core;
using NnUtils.Scripts;
using TMPro;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace InteractiveComponents
{
    [RequireComponent(typeof(TMP_Text))]
    public class InteractiveTextScript : MonoBehaviour
    {
        // Used to find custom properties within config text, e.g. {sh()}
        private const string TextRegexString = @"(?<!\\)\{(\w+)\((.*?)\)\}";
        private static readonly Regex TextRegex = new(TextRegexString);
        
        private static GeneralConfigData _configData;
        private static GeneralConfigData ConfigData => _configData ??= (GeneralConfigData)GameManagerScript.ConfigScript.Data;

        // Unprocessed text
        private string _text;
        
        [SerializeField] private TMP_Text _tmpText;
        [SerializeField] private TMP_FontAsset _defaultFont;
        
        private void Reset()
        {
            _tmpText = GetComponent<TMP_Text>();
        }
        
        private void Start()
        {
            _text = _tmpText.text;
            OnConfigLoaded();
            ConfigData.OnLoaded += OnConfigLoaded;
        }

        private void OnDestroy()
        {
            ConfigData.OnLoaded -= OnConfigLoaded;
        }

        private void OnConfigLoaded()
        {
            UpdateFont();
        }

        private void UpdateFont()
        {
            var font = ConfigData.Font;
            var asset = font == "Default" ? _defaultFont : SystemTMP.GenerateFontFromName(font);
            _tmpText.font = asset;
        }

        public void UpdateText(string text)
        {
            // Return if text hasn't changed
            if (_text == text) return;
            
            // Assign new text
            _text = text;
            _tmpText.text = ProcessText(text);
        }
        
        private string ProcessText(string text)
        {
            Debug.Log("Here");
            return TextRegex.Replace(text, match =>
            {
                var cmd = match.Groups[1].Value;
                var param = match.Groups[2].Value;

                Debug.Log($"CMD {cmd} | Param: {param}");

                return cmd.ToLower() switch
                {
                    "sh" => ExecuteShellCommand(param),
                    "time" => DateTime.Now.ToString(param),
                    _ => $"Invalid command: {cmd}"
                };
            });
        }

        private string ExecuteShellCommand(string cmd)
        {
            try
            {
                // Start the shell process and pass args
                var process = new Process();
                process.StartInfo = new()
                {
                    FileName               = ConfigData.Shell,
                    Arguments              = $"-c \"{cmd}\"",
                    RedirectStandardOutput = true,
                    RedirectStandardError  = true,
                    UseShellExecute        = false,
                    CreateNoWindow         = true
                };

                // Start the process and get output
                process.Start();
                var output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
                
                // Return the result
                return output;
            }
            catch (Exception ex)
            {
                // Return the exception for easier debugging
                return $"Error executing command: {ex.Message}";
            }
        }
    }
}