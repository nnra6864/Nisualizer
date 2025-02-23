using UnityEngine;
using System;
using System.Runtime.InteropServices;
 
namespace Scripts.Core
{
    public class WindowManager : MonoBehaviour
    {
#if UNITY_STANDALONE_WIN
        // Import necessary Windows API functions
        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
    
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
    
        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);
    
        [DllImport("user32.dll")]
        private static extern uint GetWindowLong(IntPtr hWnd, int nIndex);
 
        // Special Windows API constants
        private static readonly IntPtr HWND_BOTTOM = new IntPtr(1);
        private const int GWL_EXSTYLE = -20;
        private const uint WS_EX_LAYERED = 0x80000;
        private const uint WS_EX_TRANSPARENT = 0x20;
        private const uint SWP_NOMOVE = 0x0002;
        private const uint SWP_NOSIZE = 0x0001;
        private const uint SWP_NOACTIVATE = 0x0010;
        private const uint SWP_SHOWWINDOW = 0x0040;
 
        private IntPtr unityWindow;
        public WindowLayer Layer { get; private set; }
#endif

        private void Awake()
        {
#if UNITY_STANDALONE_WIN
            // Get the Unity window handle
            unityWindow = FindWindow(null, Application.productName);
            
            if (unityWindow == IntPtr.Zero)
            {
                Debug.LogError("Could not find Unity window!");
                return;
            }
#endif
        }
 
        public void SwitchLayer(WindowLayer layer)
        {
#if UNITY_STANDALONE_WIN
            if (unityWindow == IntPtr.Zero) return;

            Layer = layer;
 
            if (Layer == WindowLayer.Background)
            {
                // Set window to be transparent to mouse clicks
                uint exStyle = GetWindowLong(unityWindow, GWL_EXSTYLE);
                SetWindowLong(unityWindow, GWL_EXSTYLE, exStyle | WS_EX_LAYERED | WS_EX_TRANSPARENT);
 
                // Move window to bottom
                SetWindowPos(unityWindow, HWND_BOTTOM, 0, 0, 0, 0,
                    SWP_NOMOVE | SWP_NOSIZE | SWP_NOACTIVATE | SWP_SHOWWINDOW);
            }
            else
            {
                // Remove transparent flags
                uint exStyle = GetWindowLong(unityWindow, GWL_EXSTYLE);
                SetWindowLong(unityWindow, GWL_EXSTYLE, exStyle & ~(WS_EX_LAYERED | WS_EX_TRANSPARENT));
 
                // Move window back to normal position
                SetWindowPos(unityWindow, IntPtr.Zero, 0, 0, 0, 0,
                    SWP_NOMOVE | SWP_NOSIZE | SWP_SHOWWINDOW);
            }
#endif
        }
    }
}