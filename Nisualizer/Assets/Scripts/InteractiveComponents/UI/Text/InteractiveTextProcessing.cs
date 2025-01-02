using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using Config;
using Core;
using UnityEngine;

namespace InteractiveComponents.UI.Text
{
    public class InteractiveTextProcessing : MonoBehaviour
    {
        // General Config Data
        private static GeneralConfigData _configData;
        private static GeneralConfigData ConfigData => _configData ??= (GeneralConfigData)GameManagerScript.ConfigScript.Data;
        
        // Used to find custom properties within config text, e.g. {sh()}
        private const string TextRegexString = @"(?<!\\)\{(\w+)\((.*?)\)(?:,\s*(\d*\.?\d+))?\}";
        private static readonly Regex TextRegex = new(TextRegexString, RegexOptions.Compiled);

        /// Returns a list containing all the dynamic text instances
        public static List<DynamicText> GetDynamicText(string text) =>
            TextRegex.Matches(text).Select(x => new DynamicText(
                    text: "",
                    func: GetFunc(x.Groups[1].Value, x.Groups[2].Value),
                    interval: x.Groups[3].Success ? GetInterval(x.Groups[3].Value) : null))
                .ToList();
        
        /// Returns a function based on cmd
        private static Func<string> GetFunc(string cmd, string param) =>
            cmd.ToLower() switch
            {
                "sh" => () => ExecuteShellCommand(param),
                "dt" => () => DateTime.Now.ToString(param),
                _ => () => $"Invalid command: {cmd}"
            };

        /// Returns the interval
        private static float GetInterval(string interval)
        {
            float.TryParse(interval, out var result);
            return result;
        }

        /// Replaces all instances of dynamic text with proper values
        public static string ReplaceWithDynamicText(string text, Queue<DynamicText> dynamicText) =>
            TextRegex.Replace(text, _ => dynamicText.Dequeue().Text);

        /// Executes a shell command
        private static string ExecuteShellCommand(string cmd)
        {
            try
            {
                // Start the shell process and pass args
                using var process = new Process();
                process.StartInfo = new()
                {
                    FileName               = ConfigData.Shell,
                    Arguments              = $"-c \"{cmd}\"",
                    RedirectStandardOutput = true,
                    RedirectStandardError  = true,
                    UseShellExecute        = false,
                    CreateNoWindow         = true
                };

                // Start the process, get output and close the process
                process.Start();
                var output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
                process.Close();
                
                // Return the result
                return output.Trim();
            }
            catch (Exception ex)
            {
                // Return the exception for easier debugging
                return $"Error executing command: {ex.Message}";
            }
        }
    }
}