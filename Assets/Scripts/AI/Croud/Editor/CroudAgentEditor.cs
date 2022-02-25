using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Aokoro.AI.Paths;

namespace Aokoro.AI.Crouds
{
    [CustomEditor(typeof(CroudAgent), true)]
    public class CroudAgentEditor : UnityEditor.Editor
    {
        public void OnSceneGUI()
        {
            for (int i = 0; i < targets.Length; i++)
            {
                CroudAgent agent = targets[i] as CroudAgent;
                CroudAgentController controller = agent.Controller as CroudAgentController;
                if (controller != null)
                {
                    if (controller.GetPath(agent, out AIAgentPath path))
                    {
                        Handles.color = Color.yellow;
                        Handles.DrawLine(path.DestinationPosition, agent.Position);
                        Handles.Label(path.DestinationPosition + Vector3.up, $"{path.CurrentPathStep} / {path.PathSize}", EditorStyles.boldLabel);
                    }

                    if (agent.navMeshAgent.hasPath)
                    {
                        UnityEngine.AI.NavMeshPath path1 = agent.navMeshAgent.path;

                        switch (path1.status)
                        {
                            case UnityEngine.AI.NavMeshPathStatus.PathComplete:
                                Handles.color = Color.green;
                                break;
                            case UnityEngine.AI.NavMeshPathStatus.PathPartial:
                                Handles.color = Color.red;
                                break;
                            case UnityEngine.AI.NavMeshPathStatus.PathInvalid:
                                Handles.color = Color.yellow;
                                break;
                        }

                        Handles.DrawPolyLine(path1.corners);
                    }
                }
            }
        }
    }
}