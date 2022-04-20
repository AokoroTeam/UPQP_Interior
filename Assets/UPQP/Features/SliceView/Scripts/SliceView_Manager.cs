using NaughtyAttributes;
using UnityEngine;
using Cinemachine;
using System.Collections;
using UPQP.Managers;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

namespace UPQP.Features.SliceView
{
    [AddComponentMenu("UPQP/Features/SliceView/Manager")]
    public class SliceView_Manager : FeatureComponent<SliceView>
    {
        public Transform cameraCenter;
        public CinemachineVirtualCamera virtualCamera;

        [SerializeField, BoxGroup("Level")]
        private Transform levelRoot;
        [SerializeField, BoxGroup("Level")]
        private GameObject[] hide;


        [SerializeField, BoxGroup("Notification")]
        private string title = "Nouvelle fonctionnalité !";
        [SerializeField, BoxGroup("Notification")]

        private string description = "Appuyez sur la touche 1 pour activer la vue découpée et observer l'environnement dans sa globalité.";

        protected override void Start()
        {
            base.Start();
            //EnhancedTouchSupport.Enable();
        }

        private void Update()
        {
            //Debug.Log($"Fingers : {Touch.activeFingers.Count} | Touches : {Touch.activeTouches.Count}");
        }

        [Button]
        protected override void OnFeatureComponentInitiate()
        {
            if (levelRoot == null)
            {
                Debug.LogError("Please reference the root of the level", gameObject);
            }
            else
            {
                levelRoot = levelRoot.transform;
            }
            GameNotifications.Instance.TriggerNotification(title, description, 10, 50);
        }

        public Bounds GetCurrentBounds()
        {
            Bounds bounds = new();
            var meshes = levelRoot.GetComponentsInChildren<MeshRenderer>();
            for (int i = 0; i < meshes.Length; i++)
            {
                if (i == 0)
                    bounds = meshes[i].bounds;
                else
                    bounds.Encapsulate(meshes[i].bounds);
            }

            return bounds;
        }

        public void OnFeatureEnables()
        {
            _Feature.UI.ShowCommands();
            for (int i = 0; i < hide.Length; i++)
                hide[i]?.SetActive(false);

            virtualCamera.enabled = true;

        }

        public void OnFeatureDisables()
        {
            _Feature.UI.HideCommands();
            for (int i = 0; i < hide.Length; i++)
                hide[i]?.SetActive(true);

            virtualCamera.enabled = false;
        }

    }
}
