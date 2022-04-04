using Aokoro.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UPQP.Managers;

namespace UPQP.Features.SliceView
{

    public class SliceView : Feature
    {
        public override string FeatureName => "Vue coupée";
        public SliceView_Player Player { get; private set; }
        public SliceView_Manager Manager { get; private set; }
        public SliceView_UI UI { get; private set; }

        private GameObject P_PlayerComponent;
        private GameObject P_Manager;
        private GameObject P_UI;

        public SliceView(GameObject P_PlayerComponent, GameObject P_Manager, GameObject P_UI)
        {
            this.P_PlayerComponent = P_PlayerComponent;
            this.P_Manager = P_Manager;
            this.P_UI = P_UI;
        }

        protected override void GenerateNeededContentOnSetup(LevelManager manager)
        {
            //Add Manager
            Manager = GameObject.FindObjectOfType<SliceView_Manager>(false);
            //Add player Component
            Player = GameObject.Instantiate(P_PlayerComponent, manager.Player.FeaturesRoot).GetComponent<SliceView_Player>();
            //Add UI
            UI = GameObject.Instantiate(P_UI, GameUIManager.MainUI.WindowsParent).GetComponent<SliceView_UI>();

            Manager._Feature = this;
            Player._Feature = this;
            UI._Feature = this;
        }

        public override void CleanContentOnDestroy(LevelManager controller)
        {
            GameObject.Destroy(Player);
            GameObject.Destroy(Manager.gameObject);
            GameObject.Destroy(UI.gameObject);
        }

        public override void EnableFeature()
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;

            UI.ShowCommands();
            Manager.OnFeatureEnables();
            Player.OnFeatureEnables();
        }

        public override void DisableFeature()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            UI.HideCommands();
            Manager.OnFeatureDisables();
            Player.OnFeatureDisables();
        }

    }
}
