using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aokoro.UIManagement.ControlDisplay
{
    public struct CD_Command
    {
        public string actionName;
        private List<CD_CommandCombinaison> combinaisons;


        public CD_Command(string actionName)
        {
            this.actionName = actionName;
            combinaisons = new List<CD_CommandCombinaison>();
        }

        public void Add(string[] bindings, GameObject[] prefabs)
        {
            combinaisons.Add(new CD_CommandCombinaison(bindings, prefabs));
        }

        public struct CD_CommandCombinaison
        {
            public Dictionary<string, GameObject> bindings;

            public CD_CommandCombinaison(string[] bindingsPaths, GameObject[] bindingRepresentationPrefabs)
            {
                int length = Mathf.Min(bindingsPaths.Length, bindingRepresentationPrefabs.Length);
                bindings = new Dictionary<string, GameObject>(length);

                for (int i = 0; i < length; i++)
                    bindings.Add(bindingsPaths[i], bindingRepresentationPrefabs[i]);
            }
        }
    }

}
