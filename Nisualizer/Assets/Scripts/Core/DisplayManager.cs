using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Core
{
    public class DisplayManager : MonoBehaviour
    {
        private const string DebugPrefix = "DisplayManager: ";

        public static void MoveToDisplay(int displayIndex = 0, Vector2Int position = new())
        {
            if (displayIndex < 0)
            {
                Debug.LogError($"{DebugPrefix}Display Index is less than 0, returning: {displayIndex}");
                return;
            }

            List<DisplayInfo> displays = new();
            Screen.GetDisplayLayout(displays);
            
            if (displayIndex >= displays.Count)
            {
                Debug.LogError($"{DebugPrefix}Display Index is too high, returning: {displayIndex}");
                return;
            }

            Screen.MoveMainWindowTo(displays[displayIndex], position);
        }
    }
}