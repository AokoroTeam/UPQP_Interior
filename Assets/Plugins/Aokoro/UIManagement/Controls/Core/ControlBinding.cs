using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aokoro.UIManagement.Controls
{
    public struct ControlBindings
    {
        private GameObject[] prefabs;
        private string[] bindings;

        public int Lenght;
        public ControlBindings(GameObject[] prefabs, string[] bindings)
        {
            this.prefabs = prefabs;
            this.bindings = bindings;

            Lenght = Mathf.Min(prefabs.Length, bindings.Length);
        }

        public GameObject GetPrefab(int index) => prefabs[index];
        public string Getbinding(int index) => bindings[index];
    }
}
