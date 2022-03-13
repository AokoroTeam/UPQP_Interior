using Aokoro.UI.ControlsDiplaySystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Michsky.UI.ModernUIPack;
using Aokoro.UI;

namespace UPQP.Features.SliceView
{
    [AddComponentMenu("UPQP/Features/SliceView/UI")]
    public class SliceView_UI : FeatureComponent<SliceView>
    {
        private const string windowName = "SliceViewWindow";

        CD_Displayer displayer;
        WindowManager windowManager;
        int lastWindow;

        protected override void Awake()
        {
            displayer = GetComponent<CD_Displayer>();
            base.Awake();
        }

        protected override void Initiate()
        {
            windowManager = GameUIManager.WindowManager;
            WindowManager.WindowItem window = new WindowManager.WindowItem();

            window.windowName = windowName;
            window.windowObject = gameObject;

            windowManager.windows.Add(window);
            displayer.AssignActionProvider(_Feature.Player, false);
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