using Michsky.UI.ModernUIPack;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace Aokoro.UI
{
    [ExecuteInEditMode, DefaultExecutionOrder(-90)]
    public class GameUIManager : MonoBehaviour
    {
        public static GameUIManager MainUI;
        public Action OnUpdate;
        public Transform WindowsParent;
        [SerializeField, Dropdown(nameof(windowsNames))]
        private string defaultWindow;
        public string DefaultWindow => defaultWindow;

        [SerializeField, ReadOnly]
        private WindowManager windowManager;

        private List<string> windowsNames() => windowManager != null ? windowManager.windows.Select(ctx => ctx.windowName).ToList() :
         new List<string>() { "No WindowManager" };

        private void OnValidate()
        {
            windowManager = GetComponent<WindowManager>();
        }

        private void Awake()
        {
            OnValidate();
        }

        private void Start()
        {
            if (!string.IsNullOrWhiteSpace(DefaultWindow))
                OpenWindow(defaultWindow);
        }
        private void Update()
        {
            OnUpdate?.Invoke();
        }

        public void OpenWindow(string windowName) => windowManager.OpenWindow(windowName);
        public void ShowCurrentWindow() => windowManager.ShowCurrentWindow();
        public void HideCurrentWindow() => windowManager.HideCurrentWindow();
        public WindowManager.WindowItem CurrentWindow() => windowManager.windows[windowManager.currentWindowIndex];
        public WindowManager.WindowItem AddWindow(string windowName, GameObject windowObject)
        {
            WindowManager.WindowItem window = new WindowManager.WindowItem();

            window.windowName = windowName;
            window.windowObject = windowObject;

            windowManager.windows.Add(window);

            return window;
        }

        public WindowManager.WindowItem GetWindow(string windowName)
        {
            int index = windowManager.windows.FindIndex(ctx => ctx.windowName == windowName);
            return index == -1 ? null : windowManager.windows[index];
        }
    }
}