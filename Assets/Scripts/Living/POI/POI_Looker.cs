using Aokoro.Entities;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace UPQP.Environnement.Intrests
{
    public class POI_Looker : MonoBehaviour, IUpdateEntityComponent
    {
        string IEntityComponent.ComponentName => "PlayerPOILooker";
        [SerializeField]
        private Transform root;
        [SerializeField]
        private Transform target;

        [SerializeField]
        private Vector3 neutralOffset;
        [SerializeField]
        private Vector3 orientation = Vector3.forward;
        [SerializeField]
        private float damp;

        public bool HasPOI => currentPOI != null;


        [SerializeField, ReadOnly]
        private List<POI> surroundingPois;
        private POI currentPOI;

        [SerializeField]
        private float maxHeadAngle = 45;
        [SerializeField]
        private MultiAimConstraint multiAimConstraint;

        private void Awake()
        {
            surroundingPois = new List<POI>();
        }

        public void AimPoi()
        {
            Vector3 position = root.TransformPoint(neutralOffset);
            float targetWeight = 0;

            if (HasPOI)
            {
                Vector3 scaling = new Vector3(1, 0, 1);

                Vector3 poiPosition = currentPOI.LookAt.position;
                Vector3 toPOI = poiPosition - root.transform.position;

                Vector3 dir = root.TransformDirection(orientation);

                toPOI.Scale(scaling);
                dir.Scale(scaling);

                float angle = Vector3.Angle(dir, toPOI);

                if (angle < maxHeadAngle)
                {
                    position = poiPosition;
                    targetWeight = 1;
                }
            }

            multiAimConstraint.weight = Mathf.MoveTowards(multiAimConstraint.weight, targetWeight, Time.deltaTime * 2f);
            target.position = Vector3.Lerp(target.position, position, damp * Time.deltaTime);
        }

        private void OnListUpdate()
        {
            if (surroundingPois.Count == 0)
                currentPOI = null;
            else
            {
                POI nearest = null;
                float smallestDistance = 0;

                foreach (POI poi in surroundingPois)
                {
                    if (nearest == null)
                    {
                        nearest = poi;
                        break;
                    }

                    float sqr_distance = (poi.LookAt.position - root.position).sqrMagnitude;
                    if (smallestDistance > sqr_distance)
                    {
                        smallestDistance = sqr_distance;
                        nearest = poi;
                    }
                }

                currentPOI = nearest;
            }
        }

        public void RegisterPoi(POI poi)
        {
            surroundingPois.Add(poi);
            OnListUpdate();
        }

        public void UnRegisterPoi(POI poi)
        {
            if (surroundingPois.Remove(poi))
                OnListUpdate();
        }

        private void OnDrawGizmos()
        {
            if (root != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(root.TransformPoint(neutralOffset), .05f);
            }
        }

        public void OnUpdate()
        {
            AimPoi();
        }
    }
}
