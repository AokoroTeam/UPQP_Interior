using Aokoro.UI.ControlsDiplaySystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UPQP.Managers;
using Michsky.UI.ModernUIPack;



namespace UPQP.Features.SliceView
{
    public class SliceView_UI : FeatureComponent<SliceView>
    {
        private const string windowName = "SliceViewWindow";

        CD_Displayer displayer;
        WindowManager windowManager;
        int lastWindow;

        private void Awake()
        {
            displayer = GetComponent<CD_Displayer>();

        }

        protected override void Initiate()
        {
            windowManager = GameManager.Instance.WindowManager;
            WindowManager.WindowItem window = new WindowManager.WindowItem();

            window.windowName = windowName;
            window.windowObject = gameObject;

            windowManager.windows.Add(window);
            displayer.ActionProvider = _Feature.Player;
        }

        public void ShowCommands()
        {
            lastWindow = windowManager.currentWindowIndex;
            windowManager.OpenWindow(windowName);
            displayer.Show();
        }

        public void HideCommands()
        {
            displayer.Hide();
            windowManager.OpenWindowByIndex(lastWindow);
        }
    }
}