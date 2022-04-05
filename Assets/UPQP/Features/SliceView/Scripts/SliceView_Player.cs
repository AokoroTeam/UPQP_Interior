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

        InputAction moveAction;
        InputAction rotateAction;
        InputAction zoomAction;

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

            cameraCenter.position = LevelBounds.center;

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
                rotateAction = map.FindAction("Rotate");
                zoomAction = map.FindAction("Zoom");
                moveAction = map.FindAction("Move");

                ///Rotation
                rotateAction.performed += OnRotate_performed;
                ///Zoom
                zoomAction.performed += OnZoom_Performed;

                ///Move
                moveAction.performed += OnMove;

                map.FindAction("Exit").performed += OnExit_performed;
            }
            catch (System.Exception e)
            {
                Debug.LogException(e);
            }
        }

        private void OnMoveInitiated(InputAction.CallbackContext ctx)
        {
            if (ctx.phase == InputActionPhase.Started)
                moveAction.Enable();

            if (ctx.phase == InputActionPhase.Performed)
                moveAction.Disable();
        }

        private void OnMove(InputAction.CallbackContext ctx)
        {
            //Pixel positions
            Vector2 delta = ctx.ReadValue<Vector2>();
            Vector2 pixelPositionOfCenter = Camera.main.ViewportToScreenPoint(Vector3.one * .5f);

            //Rays
            Ray centerRay = Camera.main.ScreenPointToRay(pixelPositionOfCenter);
            Ray deltaRay = Camera.main.ScreenPointToRay(pixelPositionOfCenter + delta);

            //Creating plane
            Plane plane;
            if (Physics.Raycast(centerRay, out RaycastHit centerHit))
                plane = new Plane(Vector3.up, centerHit.point);
            else
                plane = new Plane(Vector3.up, LevelBounds.center);

            //Dragging
            if (plane.Raycast(centerRay, out float centerDistance)
                && plane.Raycast(deltaRay, out float deltaDistance))
            {
                Vector3 A = centerRay.GetPoint(centerDistance);
                Vector3 B = deltaRay.GetPoint(deltaDistance);

                Debug.DrawRay(A, Vector3.up * 10, Color.blue);
                Debug.DrawRay(B, Vector3.up * 10, Color.green);

                Vector3 worldDelta = B - A;
                //Debug.Log($"A : {A} | B : {B}");

                cameraCenter.position -= worldDelta;
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