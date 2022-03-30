using Aokoro.Entities.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using NaughtyAttributes;
using Aokoro.Entities;
using UPQP.Player;

namespace UPQP.Features.SliceView
{
    [AddComponentMenu("UPQP/Features/SliceView/Player")]
    public class SliceView_Player : PlayerFeatureComponent<SliceView>//, IUpdateEntityComponent<UPQP.Player.UPQP_Player>
    {
        [SerializeField, BoxGroup("Rotation")]
        private float rotationSpeedModifier = 10;
        [SerializeField, BoxGroup("Rotation")]
        private CinemachineOrbitalTransposer vCam;


        [SerializeField, BoxGroup("zoom"), MinMaxSlider(0, 200)]
        Vector2 zoom;


        private CinemachineFollowZoom zoomComponent;
        private Bounds LevelBounds;

        public UPQP_Player Manager { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public string ComponentName => throw new System.NotImplementedException();

        protected override void OnFeatureComponentInitiate()
        {
            
        }

        protected override void Start()
        {
            vCam = _Feature.Manager.virtualCamera.GetCinemachineComponent<CinemachineOrbitalTransposer>();
            zoomComponent = vCam.VirtualCamera.GetComponent<CinemachineFollowZoom>();

            zoomComponent.m_MinFOV = zoom.x;
            zoomComponent.m_MaxFOV = zoom.y;
            base.Start();
        }


        public override void OnFeatureEnables()
        {
            Player.ChangeActionMap(MapName);
            Player.Freezed.Subscribe(this, 20, true);

            if (LevelBounds == null)
                LevelBounds = _Feature.Manager.GetCurrentBounds();
        }

        public override void OnFeatureDisables()
        {
            Player.ChangeActionMap(Player.DefaultActionMap);
            Player.Freezed.Unsubscribe(this);
        }

        public override void BindToNewActions(InputActionAsset asset)
        {
            var map = asset.FindActionMap(MapName);
            ///Rotation
            map.FindAction("Rotate").performed += OnRotate_performed;
            map.FindAction("Rotate").canceled += OnRotate_performed;
            map.FindAction("Rotate").started += OnRotate_performed;

            ///Zoom
            map.FindAction("Zoom").performed += OnZoom_Performed;
            map.FindAction("Zoom").canceled += OnZoom_Performed;
            map.FindAction("Zoom").started += OnZoom_Performed;

            map.FindAction("Exit").performed += OnExit_performed;
        }

        private void OnExit_performed(InputAction.CallbackContext ctx) => Player.EndFeature(_Feature);

        private void OnRotate_performed(InputAction.CallbackContext ctx)
        {
            vCam.m_XAxis.m_InputAxisValue = ctx.ReadValue<float>() * rotationSpeedModifier * Time.deltaTime;
        }

        private void OnZoom_Performed(InputAction.CallbackContext ctx)
        {

        }
    }
}