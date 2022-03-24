using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aokoro.UI.ControlsDiplaySystem
{
    [System.Serializable]
    public struct CD_InputControl
    {
        public bool IsComposite => size > 1;
        public string Path => Paths[0];
        public string DisplayName => DisplayNames[0];

        public string compositeType;

        public string[] Paths;
        public string[] DisplayNames;

        public int size;

        public CD_InputControl(string path, string displayName)
        {
            this.size = 1;
            this.compositeType = string.Empty;

            Paths = new string[] { path };
            DisplayNames = new string[] { displayName };
        }

        public CD_InputControl(int size, string compositeType)
        {
            this.size = size;
            this.compositeType = compositeType;

            Paths = new string[size];
            DisplayNames = new string[size];
        }

        public void SetAtIndex(int index, string path, string displayName)
        {
            Paths[index] = path;
            DisplayNames[index] = displayName;
        }
    }
}
