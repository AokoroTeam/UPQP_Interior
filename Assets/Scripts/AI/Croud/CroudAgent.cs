using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Aokoro.Pooling;
using Aokoro.AI.Paths;
using NaughtyAttributes;
using Aokoro.AI;
using UPQP.Environnement.Intrests;

namespace Aokoro.AI.Crouds
{

    [AddComponentMenu("Aokoro/AI/Croud/Base Agent")]
    public class CroudAgent : AIAgent, IPoolObjectCallbackReceiver
    {
        [HideInInspector]
        public NavMeshAgent navMeshAgent;
        [BoxGroup("Settings"), SerializeField] Vector2Int minMaxAvoidancePriority = new Vector2Int(10, 30);
        [BoxGroup("Settings"), SerializeField] Vector2Int minMaxSpeed = new Vector2Int(1, 4);
        [BoxGroup("Settings"), SerializeField] float defaultSpeed = 2;


        public PoolObject poolObject { get; set; }
        public CroudAgentController agentController => Controller as CroudAgentController;

        protected Animator Anim => currentSkin.animator;

        private CroudSkin[] skins;

        public CroudSkin currentSkin;

        bool firstPass = true;

        protected override void Awake()
        {
            base.Awake();

            navMeshAgent = GetComponent<NavMeshAgent>();
            skins = GetComponentsInChildren<CroudSkin>();
        }

        public bool HasPath => navMeshAgent.hasPath;
        public Vector3 Destination { get => navMeshAgent.destination; set => navMeshAgent.SetDestination(value); }


        public override Vector3 MoveAgentTo(Vector3 position)
        {
            if (navMeshAgent.Warp(position))
                return position;

            return base.MoveAgentTo(position);
        }

        public void Animate()
        {
            Vector3 localVelocity = transform.InverseTransformDirection(navMeshAgent.velocity);
            Anim.SetFloat("Forward_Speed", localVelocity.z);
        }

        #region Pool
        public override void BeforeAwake()
        {
            if (firstPass)
            {
                int skinsCount = skins.Length;
                int rng = Random.Range(0, skinsCount);

                for (int i = 0; i < skinsCount; i++)
                {
                    if (i == rng)
                        skins[i].gameObject.SetActive(true);
                    else
                        Destroy(skins[i].gameObject);
                }

                currentSkin = skins[rng];
                currentSkin.RandomizeSkin();
            }

            navMeshAgent.avoidancePriority = Random.Range(minMaxAvoidancePriority.x, minMaxAvoidancePriority.y);
            navMeshAgent.speed = Random.Range(minMaxSpeed.x, minMaxSpeed.y);

            Anim.SetFloat("AnimationSpeedMultiplier", navMeshAgent.speed / defaultSpeed);
            GetComponentInChildren<POI>().priority = Random.Range(30, 50);

            firstPass = false;
        }


        public override void BeforeDestroy()
        {
        }
        #endregion

        public override bool IsDespawnabled() => false;

        void IPoolObjectCallbackReceiver.OnPoolAwake()
        {
        }

        void IPoolObjectCallbackReceiver.OnPoolDestroy()
        {
        }
    }
}