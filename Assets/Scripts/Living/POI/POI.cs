using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace UPQP.Environnement.Intrests
{
    [RequireComponent(typeof(Collider))]
    public class POI : MonoBehaviour
    {
        public Transform LookAt => lookAt;

        [Range(0, 200)]
        public int priority;
        [SerializeField]
        private Transform lookAt;

        [SerializeField, Tag]
        private string[] tags;



        private void OnTriggerEnter(Collider other)
        {
            int length = tags.Length;

            if (length > 0)
            {
                bool accepted = false;
                for (int i = 0; i < length; i++)
                {
                    if (other.CompareTag(tags[i]))
                    {
                        accepted = true;
                        break;
                    }
                }
                if (!accepted)
                    return;
            }

            if (other.TryGetComponent(out POI_Looker looker))
                looker.RegisterPoi(this);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out POI_Looker looker))
                looker.UnRegisterPoi(this);
        }
    }
}
