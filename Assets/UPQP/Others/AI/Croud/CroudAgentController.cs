using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;
using Aokoro.Pooling;
using Aokoro.AI.Grid;
using Aokoro.AI.Paths;
using NaughtyAttributes;

namespace Aokoro.AI.Crouds
{
    [AddComponentMenu("Aokoro/AI/Croud/CroudAgentController")]
    public class CroudAgentController : AIAgentController<CroudAgent, AIGrid>
    {
        [BoxGroup("Agents"), SerializeField] PoolResource agentsPrefab;


        [BoxGroup("Paths")]
        [SerializeField] private float arrivingDistance;
        [BoxGroup("Paths")]
        [SerializeField, Min(1)] private int pathLenght = 1;

        private Pool agentPool;
        private AIWaypointWire wire;
        private Dictionary<CroudAgent, AIAgentPath> paths = new Dictionary<CroudAgent, AIAgentPath>();

        private bool IsFirstFrame;

        protected override void Awake()
        {
            IsFirstFrame = true;
            wire = GetComponentInChildren<AIWaypointWire>();
            wire.BuildWire();

            agentPool = Pool.Create(gameObject, MaxAgent, agentsPrefab);
            base.Awake();
        }

        private void Start()
        {
            IsFirstFrame = false;
        }

        protected override CroudAgent InstantiateNewAgent()
        {
            if (agentPool.Capacity != MaxAgent)
                agentPool.UpdateCapacity(MaxAgent);

            var PO = agentPool.POInstantiate(transform);

            return PO.GetComponent<CroudAgent>();
        }

        protected override void SetupAgent(CroudAgent agent)
        {
            if (IsFirstFrame)
            {
                //Random distribution
                Vector3 position = grid.GetRandomPosition();
                agent.MoveAgentTo(position);
            }
            else
            {
                Vector3 position = wire.GetNextWaypointFrom(grid.GetRandomPosition(), WaypointBehavior.Spawn, agent.navMeshAgent.areaMask).GetPositionInsideZone();
                agent.MoveAgentTo(position);
            }

            paths.Add(agent, new AIAgentPath(agent, wire, pathLenght, agent.navMeshAgent.areaMask));
        }

        protected override void DestroyAgent(CroudAgent agent)
        {
            if (agentPool.Capacity != MaxAgent)
                agentPool.UpdateCapacity(MaxAgent);

            agentPool.PODestroy(agent.poolObject);
        }

        protected override void UpdateAgent(CroudAgent agent)
        {
            HandleAgentPath(agent);
            agent.Animate();
        }

        private void HandleAgentPath(CroudAgent agent)
        {
            NavMeshAgent navMeshAgent = agent.navMeshAgent;
            if (GetPath(agent, out AIAgentPath path))
            {
                Vector3 destination = path.DestinationPosition;

                if (!navMeshAgent.hasPath || destination != navMeshAgent.destination)
                    navMeshAgent.SetDestination(destination);

                float sqrDistance = (destination - agent.Position).sqrMagnitude;

                if (sqrDistance < arrivingDistance * arrivingDistance && !path.MoveNext())
                    RemoveAgent(agent);
            }
        }
        protected override void RemoveAgent(CroudAgent agent)
        {
            base.RemoveAgent(agent);
            paths.Remove(agent);
        }

        public bool GetPath(CroudAgent agent, out AIAgentPath path) => paths.TryGetValue(agent, out path);
    }
}
