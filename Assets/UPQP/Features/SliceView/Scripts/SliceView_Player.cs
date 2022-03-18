using Aokoro.Entities.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using UPQP.Features;
using Aokoro.UI;

namespace UPQP.Features.SliceView
{
    [AddComponentMenu("UPQP/Features/SliceView/Player")]
    public class SliceView_Player : PlayerFeatureComponent<SliceView>
    {
        private CinemachineOrbitalTransposer vCam;

        protected override void Initiate()
        {
        }

        protected override void Start()
        {
            vCam = _Feature.Manager.virtualCamera.GetCinemachineComponent<CinemachineOrbitalTransposer>();
            base.Start();
        }


        public override void OnFeatureEnables()
        {
            Player.ChangeActionMap(MapName);
            Player.Freezed.Subscribe(this, 20, true);
        }

        public override void OnFeatureDisables()
        {
            Player.ChangeActionMap(Player.DefaultActionMap);
            Player.Freezed.Unsubscribe(this);
        }

        public override void BindToNewActions(InputActionAsset asset)
        {
            var map = asset.FindActionMap(MapName);
            map.FindAction("Rotate").performed += OnRotate_performed;
            map.FindAction("Rotate").canceled += OnRotate_performed;
            map.FindAction("Rotate").started += OnRotate_performed;

            map.FindAction("Exit").performed += OnExit_performed;
        }

        private void OnExit_performed(InputAction.CallbackContext ctx) => Player.EndFeature(Feature);

        private void OnRotate_performed(InputAction.CallbackContext ctx) => vCam.m_XAxis.m_InputAxisValue = ctx.ReadValue<float>() * Time.deltaTime;

    }
}