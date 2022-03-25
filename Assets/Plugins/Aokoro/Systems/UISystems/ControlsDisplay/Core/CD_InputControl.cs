using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aokoro.UI.ControlsDiplaySystem
{
    [System.Serializable]
    public struct CD_InputControl : IEnumerable<CD_InputControl.CD_ControlData>
    {
        public readonly struct CD_ControlData
        {
            public readonly string Path;
            public readonly string DisplayName;

            internal CD_ControlData(string path, string displayName)
            {
                Path = path;
                DisplayName = displayName;
            }
        }
        public bool IsComposite => Lenght > 1;
        public string Path => controls[0].Path;
        public string DisplayName => controls[0].DisplayName;
        public int Lenght => controls.Count;

        public string compositeType;

        private List<CD_ControlData> controls;


        public CD_InputControl(string path, string displayName)
        {
            this.compositeType = string.Empty;

            controls = new List<CD_ControlData>() { new CD_ControlData(path, displayName) };
        }

        public CD_InputControl(string compositeType)
        {
            this.compositeType = compositeType;
            controls = new List<CD_ControlData>();
        }

        public void AddControl(string path, string displayName)
        {
            controls.Add(new CD_ControlData(path, displayName));
        }

        public string GetPathAtIndex(int index) => controls[index].Path;
        public string GetDisplayNameAtIndex(int index) => controls[index].DisplayName;
        public CD_ControlData GetDataAtIndex(int index) => controls[index];

        public CD_InputControl[] Split()
        {
            if (!IsComposite)
                return new CD_InputControl[] { this };
            else
            {
                CD_InputControl[] controlArray = new CD_InputControl[Lenght];
                for (int i = 0; i < Lenght; i++)
                    controlArray[i] = new CD_InputControl(GetPathAtIndex(i), GetDisplayNameAtIndex(i));

                return controlArray;
            }
        }

        public IEnumerator<CD_ControlData> GetEnumerator() => controls.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => controls.GetEnumerator();
    }
}
