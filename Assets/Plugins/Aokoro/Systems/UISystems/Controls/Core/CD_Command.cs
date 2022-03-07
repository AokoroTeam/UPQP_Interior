using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aokoro.UIManagement.ControlsDiplaySystem
{
    public struct CD_Command : IEnumerable<CD_InputCombination>
    {
        public string actionName;

        private List<CD_InputCombination> combinations;
        public int CombinationsCount => combinations.Count;


        public CD_InputCombination this[int i] => combinations[i];
        public CD_Command(string actionName)
        {
            this.actionName = actionName;
            combinations = new List<CD_InputCombination>();
        }

        public void Addcombination(params MatchedInput[] inputs)
        {
            this.combinations.Add(new CD_InputCombination(inputs));
        }

        IEnumerator<CD_InputCombination> IEnumerable<CD_InputCombination>.GetEnumerator() => combinations.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => (this as IEnumerable).GetEnumerator();


    }
    public struct CD_InputCombination : IEnumerable<MatchedInput>
    {
        private MatchedInput[] inputs;
        public CD_InputCombination(MatchedInput[] matchedInputs)
        {
            inputs = matchedInputs;
        }
        public MatchedInput this[int i] => inputs[i];
        public int Length => inputs != null ? inputs.Length : 0;

        IEnumerator<MatchedInput> IEnumerable<MatchedInput>.GetEnumerator() => inputs.GetEnumerator() as IEnumerator<MatchedInput>;

        IEnumerator IEnumerable.GetEnumerator() => (this as IEnumerable).GetEnumerator();
    }
}
