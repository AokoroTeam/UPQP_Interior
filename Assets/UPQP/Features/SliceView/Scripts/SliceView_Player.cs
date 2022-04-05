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
using System;
using UPQP.Player.Movement;

namespace UPQP.Features.SliceView
{
    [AddComponentMenu("UPQP/Features/SliceView/Player")]
    public class SliceView_Player : PlayerFeatureComponent<SliceView>, IUpdateEntityComponent<UPQP_Player>
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
        private int recenterSpeed;


        private CinemachineFollowZoom zoomComponent;
        private Transform cameraCenter;
        private Bounds LevelBounds;

        InputAction cursorPosition;
        InputAction moveAction;
        InputAction rotateAction;
        InputAction zoomAction;

        private Vector3 CenterPosition;

        public UPQP_Player Manager { get; set; }

        public string ComponentName =>"SliceView player";

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
            if(Player.GetLivingComponent(out PlayerCharacter character))
            {
                Debug.Log("[Slice view] Freezing player movements");
                character.Freezed.Subscribe(this, 20, true);
            }

            if (LevelBounds == null)
                LevelBounds = _Feature.Manager.GetCurrentBounds();

            cameraCenter.position = LevelBounds.center;
            CenterPosition = LevelBounds.center;
        }

        public override void OnFeatureDisables()
        {
            Player.ChangeActionMap(Player.DefaultActionMap);

            if (Player.GetLivingComponent(out PlayerCharacter character))
            {
                Debug.Log("[Slice view] Defreezing player movements");
                character.Freezed.Unsubscribe(this);
            }
        }

        public override void BindToNewActions(InputActionAsset asset)
        {
            var map = asset.FindActionMap(MapName);
            try
            {
                rotateAction = map.FindAction("Rotate");
                zoomAction = map.FindAction("Zoom");
                moveAction = map.FindAction("Move");
                cursorPosition = map.FindAction("CursorPosition");

                ///Rotation
                rotateAction.performed += OnRotate_performed;
                ///Zoom
                zoomAction.performed += OnZoom_Performed;

                ///Move
                moveAction.performed += OnMove;

                map.FindAction("Interact").performed += OnInteract;

                map.FindAction("Exit").performed += OnExit_performed;

            }
            catch (System.Exception e)
            {
                Debug.LogException(e);
            }
        }

        private void OnInteract(InputAction.CallbackContext ctx)
        {
            //Ray
            Ray centerRay = Camera.main.ScreenPointToRay(cursorPosition.ReadValue<Vector2>());
            if (Raycast(centerRay, out RaycastHit centerHit))
            {
                CenterPosition = centerHit.point;
            }
            else
            {
                Debug.Log("Nothing touched");
            }
        }

        private static bool Raycast(Ray centerRay, out RaycastHit centerHit)
        {
            return Physics.Raycast(centerRay, out centerHit, 100, LayerMask.GetMask("SliceViewPlane"), QueryTriggerInteraction.Collide);
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
            if (Raycast(centerRay, out RaycastHit centerHit))
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

                CenterPosition -= worldDelta;
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

        public void OnUpdate()
        {
            Debug.Log(_Feature.IsActive);
            if(_Feature.IsActive)
            {
                cameraCenter.position = Vector3.Lerp(cameraCenter.position, CenterPosition, Time.deltaTime * recenterSpeed);
            }
        }

        public void Initiate(UPQP_Player manager)
        {

        }
    }
}