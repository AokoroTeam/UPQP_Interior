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
        int lastWindow;

        protected override void Awake()
        {
            displayer = GetComponent<CD_Displayer>();
            base.Awake();
        }

        protected override void OnFeatureComponentInitiate()
        {
            GameUIManager.MainUI.AddWindow(windowName, gameObject);

            displayer.AssignActionProvider(_Feature.Player, false);
        }

        public void ShowCommands()
        {
            GameUIManager.MainUI.OpenWindow(windowName);
            displayer.Show();
        }

        public void HideCommands()
        {
            displayer.Hide();
            GameUIManager.MainUI.OpenWindow(GameUIManager.MainUI.DefaultWindow);
        }
    }
}