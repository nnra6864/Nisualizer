using UnityEngine;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Scripts.Core
{
    public class WindowManager : MonoBehaviour
    {
        public void SwitchLayer(WindowLayer layer, int displayIndex = 0)
        {
#if UNITY_STANDALONE_WIN && !UNITY_EDITOR
            // Initialize if not initialized
            if (!_init) Init();

            // Get the display
            List<DisplayInfo> displays = new();
            Screen.GetDisplayLayout(displays);
            displayIndex = displayIndex < 0 ? 0 : displayIndex >= displays.Count ? displays.Count - 1 : displayIndex;
            var display = displays[displayIndex];

            if (layer == WindowLayer.Background) MoveToBackground(display);
            else MoveToForeground(display);
#else
            Debug.LogError("Switching Window Layers is only supported on Windows.");
#endif
        }
        
#if UNITY_STANDALONE_WIN
        // Window styles
        private const int GwlStyle = -16;
        private const int GwlExStyle = -20;
        private const int WsChild = 0x40000000;
        private const int WsExToolwindow = 0x00000080;
        private const int WsExNoActivate = 0x08000000;

        // For finding desktop window
        private static readonly IntPtr HwndBottom = new(1);
        private static readonly IntPtr HwndTop = new(0);
        private const int SwpNoMove = 0x0002;
        private const int SwpNoSize = 0x0001;
        private const int SwpNoActivate = 0x0010;
        private const int SwpNoZOrder = 0x0004;
        private const int SwpShowWindow = 0x0040;

        // DLL imports for Windows API
        [DllImport("user32.dll")]
        private static extern IntPtr GetActiveWindow();

        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(string className, string windowName);

        [DllImport("user32.dll")]
        private static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string className, string windowName);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll")]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll")]
        private static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll")]
        private static extern bool EnableWindow(IntPtr hWnd, bool bEnable);

        // Delegate for EnumWindows callback
        private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern bool EnumWindows(EnumWindowsProc enumProc, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessageTimeout(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam, uint fuFlags, uint uTimeout,
            ref IntPtr lpdwResult);

        // Handles
        private IntPtr _unityWindowHandle;
        private IntPtr _progmanHandle;
        private IntPtr _workerWHandle;
        private IntPtr _desktopHandle;
        
        // Original Style
        private int _originalStyle;
        private int _originalExStyle;

        private bool _init;
        private WindowLayer _layer = WindowLayer.Foreground;

        private void Init()
        {
            // Get handle to the Unity window
            _unityWindowHandle = GetActiveWindow();
            if (_unityWindowHandle == IntPtr.Zero)
            {
                Debug.LogError("Failed to get Unity window handle.");
                return;
            }
            
            // Find the Program Manager window
            _progmanHandle = FindWindow("Progman", null);
            if (_progmanHandle == IntPtr.Zero)
            {
                Debug.LogError("Failed to find Program Manager window.");
                return;
            }
            
            // Send message to Program Manager to create the WorkerW
            var result = IntPtr.Zero;
            SendMessageTimeout(_progmanHandle, 0x052C, new IntPtr(0), IntPtr.Zero, 0x0, 1000, ref result);

            // Find the WorkerW window (this is where wallpaper engine places its window)
            _workerWHandle = IntPtr.Zero;
            EnumWindows((tophandle, topparamhandle) =>
            {
                var shellDefView = FindWindowEx(tophandle, IntPtr.Zero, "SHELLDLL_DefView", null);
                if (shellDefView == IntPtr.Zero) return true;
                
                // Get the desktop window handle
                _desktopHandle = FindWindowEx(shellDefView, IntPtr.Zero, "SysListView32", "FolderView");

                // This is the WorkerW window that contains the desktop icons
                _workerWHandle = FindWindowEx(IntPtr.Zero, tophandle, "WorkerW", null);
                return false;

            }, IntPtr.Zero);
            
            // Return if WorkerW is not found
            if (_workerWHandle == IntPtr.Zero)
            {
                Debug.LogError("Failed to find WorkerW window.");
                return;
            }
            
            // Store original style
            _originalStyle = GetWindowLong(_unityWindowHandle, GwlStyle);
            _originalExStyle = GetWindowLong(_unityWindowHandle, GwlExStyle); 
            
            _init = true;
        }

        private void MoveToBackground(DisplayInfo display)
        {
            if (_layer == WindowLayer.Background)
            {
                Debug.Log("Window is already on the background layer, returning");
                return;
            }

            // Window Style Constants
            const int wsCaption = 0x00C00000; // Window has a title bar
            const int wsThickFrame = 0x00040000; // Window has a sizing border
            const int wsMinimize = 0x20000000; // Window has a minimize button
            const int wsMaximize = 0x10000000; // Window has a maximize button
            const int wsSysMenu = 0x00080000; // Window has a system menu on its title bar

            // Modify window style to make it a child window, prevent it from activating, and remove all window decorations
            SetWindowLong(_unityWindowHandle, GwlStyle,
                (_originalStyle | WsChild) &
                ~wsCaption &
                ~wsThickFrame &
                ~wsMinimize &
                ~wsMaximize &
                ~wsSysMenu
                );
            SetWindowLong(_unityWindowHandle, GwlExStyle, _originalExStyle | WsExToolwindow | WsExNoActivate);

            // Set our Unity window to be a child of the WorkerW window
            SetParent(_unityWindowHandle, _workerWHandle);

            // Position and size the window appropriately
            var rect = display.workArea;
            SetWindowPos(_unityWindowHandle, HwndTop, 0, 0,
                rect.width, rect.height,
                SwpNoActivate | SwpShowWindow);
            Debug.Log($"Moved window to: {rect.x} | {rect.y} | {rect.width} | {rect.height}");

            _layer = WindowLayer.Background;
            Debug.Log("Successfully moved window to the background layer.");
        }

        private void MoveToForeground(DisplayInfo display)
        {
            if (_layer == WindowLayer.Foreground)
            {
                Debug.Log("Window is already on the foreground layer, returning");
                return;
            }

            // Remove the child window relationship
            SetParent(_unityWindowHandle, IntPtr.Zero);
            
            // Restore original window style
            SetWindowLong(_unityWindowHandle, GwlStyle, _originalStyle);
            SetWindowLong(_unityWindowHandle, GwlExStyle, _originalExStyle);

            // Restore window position
            SetWindowPos(_unityWindowHandle, IntPtr.Zero, 0, 0, Screen.currentResolution.width, Screen.currentResolution.height, SwpShowWindow);

            _layer = WindowLayer.Foreground;
            Debug.Log("Successfully moved window to the foreground layer.");
        }

        private void OnApplicationQuit()
        {
            if (_layer != WindowLayer.Background) return;

            // Clean up window settings when application is closed
            SetParent(_unityWindowHandle, IntPtr.Zero);
            SetWindowPos(_unityWindowHandle, IntPtr.Zero, 0, 0, 0, 0, SwpNoZOrder | SwpShowWindow);
        }
#endif
    }
}