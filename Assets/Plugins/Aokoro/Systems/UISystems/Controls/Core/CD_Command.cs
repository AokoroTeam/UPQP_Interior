using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aokoro.UIManagement.ControlsDiplaySystem
{
    public struct CD_Command : IEnumerable<CD_InputCombination>
    {
        public string actionName;

        private List<CD_InputCombination> inputs;
        public int CombinationsCount => inputs.Count;


        public CD_InputCombination this[int i] => inputs[i];
        public CD_Command(string actionName)
        {
            this.actionName = actionName;
            inputs = new List<CD_InputCombination>();
        }

        public void Addcombination(params CD_Input[] inputs)
        {
            this.inputs.Add(new CD_InputCombination(inputs));
        }

        IEnumerator<CD_InputCombination> IEnumerable<CD_InputCombination>.GetEnumerator() => inputs.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => (this as IEnumerable).GetEnumerator();


    }
    public struct CD_InputCombination : IEnumerable<CD_Input>
    {
        private CD_Input[] inputs;
        public CD_InputCombination(CD_Input[] bindingRepresentationPrefabs)
        {
            inputs = bindingRepresentationPrefabs;
        }
        public CD_Input this[int i] => inputs[i];
        public int Length => inputs == null ? inputs.Length : 0;

        IEnumerator<CD_Input> IEnumerable<CD_Input>.GetEnumerator() => inputs.GetEnumerator() as IEnumerator<CD_Input>;

        IEnumerator IEnumerable.GetEnumerator() => (this as IEnumerable).GetEnumerator();
    }
}
