using Aokoro.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UPQP.Managers;

namespace UPQP.Features.SliceView
{

    public class SliceView : Feature
    {
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

        protected override void Generate(LevelManager manager)
        {
            //Add Manager
            Manager = GameObject.Instantiate(P_Manager, GameManager.Instance.transform).GetComponent<SliceView_Manager>();
            //Add player Component
            Player = GameObject.Instantiate(P_PlayerComponent, manager.Player.FeaturesRoot).GetComponent<SliceView_Player>();
            //Add UI
            UI = GameObject.Instantiate(P_UI, GameUIManager.Instance.WindowsParent).GetComponent<SliceView_UI>();
        }

        public override void Clean(LevelManager controller)
        {
            GameObject.Destroy(Player.gameObject);
            GameObject.Destroy(Manager.gameObject);
            GameObject.Destroy(UI.gameObject);
        }
    }
}
