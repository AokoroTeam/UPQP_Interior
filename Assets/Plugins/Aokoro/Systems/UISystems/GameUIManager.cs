using Michsky.UI.ModernUIPack;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aokoro.UI
{
    [ExecuteInEditMode, DefaultExecutionOrder(-90)]
    public class GameUIManager : Singleton<GameUIManager>
    {
        public Action OnUpdate;
        public Transform WindowsParent;

        private WindowManager windowManager;

        protected override void OnExistingInstanceFound(GameUIManager existingInstance)
        {
            Destroy(gameObject);
        }
        protected override void Awake()
        {
            base.Awake();
            if(IsInstance)
                windowManager = GetComponent<WindowManager>();
        }



        private void Update()
        {
            OnUpdate?.Invoke();
        }

        public static void OpenWindow(string windowName) => Instance.windowManager.OpenWindow(windowName);
        public static void ShowCurrentWindow() => Instance.windowManager.ShowCurrentWindow();
        public static void HideCurrentWindow() => Instance.windowManager.HideCurrentWindow();
        public static WindowManager.WindowItem CurrentWindow() => Instance.windowManager.windows[Instance.windowManager.currentWindowIndex];
        public static WindowManager.WindowItem AddWindow(string windowName, GameObject windowObject)
        {
            WindowManager.WindowItem window = new WindowManager.WindowItem();

            window.windowName = windowName;
            window.windowObject = windowObject;

            Instance.windowManager.windows.Add(window);

            return window;
        }
    }
}