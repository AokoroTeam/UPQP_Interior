using Aokoro.Entities.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using NaughtyAttributes;
using Aokoro.Entities;
using UPQP.Player;
using DG.Tweening;

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
        [SerializeField, BoxGroup("zoom")]
        private float zoomDamping;
        [SerializeField, BoxGroup("zoom")]
        private int zoomSpeed;

        [SerializeField, BoxGroup("move")]
        private int moveSpeed;


        private CinemachineFollowZoom zoomComponent;
        private Transform cameraCenter;
        private Bounds LevelBounds;

        protected override void OnFeatureComponentInitiate()
        {
            
        }

        protected override void Start()
        {
            base.Start();

            vCam = _Feature.Manager.virtualCamera.GetCinemachineComponent<CinemachineOrbitalTransposer>();
            zoomComponent = vCam.VirtualCamera.GetComponent<CinemachineFollowZoom>();
            cameraCenter = _Feature.Manager.cameraCenter;

            zoomComponent.m_MinFOV = zoom.x;
            zoomComponent.m_MaxFOV = zoom.y;
            zoomComponent.m_Damping = zoomDamping;
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
            try
            {
                ///Rotation
                map.FindAction("Rotate").performed += OnRotate_performed;
                ///Zoom
                map.FindAction("Zoom").performed += OnZoom_Performed;
                ///Move
                map.FindAction("Move").performed += OnMove_Performed;
                

                map.FindAction("Exit").performed += OnExit_performed;
            }
            catch(System.Exception e)
            {
                Debug.LogException(e);
            }
        }

        private Vector3 firstClickRayPoint;
        private Plane scrollPlane;

        private void OnMove_Performed(InputAction.CallbackContext ctx)
        {

            switch(ctx.phase)
            {
                case InputActionPhase.Started:
                    Ray ray = Camera.main.ScreenPointToRay(ctx.ReadValue<Vector3>());
                    if (Physics.Raycast(ray, out RaycastHit hitInfos))
                    {
                        firstClickRayPoint = hitInfos.point;
                        scrollPlane = new Plane(Vector3.up, firstClickRayPoint);
                    }
                    else
                    {
                        scrollPlane = new Plane(Vector3.up, LevelBounds.center);
                        scrollPlane.Raycast(ray, out float distance);
                        firstClickRayPoint = ray.GetPoint(distance);
                    }
                    break;
                case InputActionPhase.Performed:
                    Ray ray = Camera.main.ScreenPointToRay(ctx.ReadValue<Vector3>());
                    break;
                case InputActionPhase.Canceled:
                    break;
            }
        }

        private void OnExit_performed(InputAction.CallbackContext ctx) => Player.EndFeature(_Feature);

        private void OnRotate_performed(InputAction.CallbackContext ctx)
        {
            vCam.m_XAxis.m_InputAxisValue = ctx.ReadValue<float>() * rotationSpeedModifier * Time.deltaTime;
        }

        private void OnZoom_Performed(InputAction.CallbackContext ctx)
        {
            float input = ctx.ReadValue<float>() * Time.deltaTime;
            float newWidth = input * zoomSpeed + zoomComponent.m_Width;
            zoomComponent.m_Width = Mathf.Clamp(newWidth, zoom.x, zoom.y);
        }
    }
}