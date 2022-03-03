using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

namespace Aokoro.UIManagement.ControlsDiplaySystem
{
    public class CD_DisplayCommand : MonoBehaviour
    {
        [SerializeField]
        TextMeshProUGUI description;
        [SerializeField]
        private Transform combinationsParents;
        [SerializeField]
        private GameObject combinationLayout;

        private CD_DisplayInputsCombination[] instantiatedCombinations;
        private bool or, and;

        public virtual void Fill(CD_Command command, bool withOr = true, bool withAnd = true)
        {
            or = withOr;
            and = withAnd;

            description.text = command.actionName;

            Clear();
            instantiatedCombinations = new CD_DisplayInputsCombination[command.CombinationsCount];
            CreateCommandDisplays(command, instantiatedCombinations);
        }


        protected virtual void CreateCommandDisplays(CD_Command command, CD_DisplayInputsCombination[] output)
        {
            for (int i = 0; i < command.CombinationsCount; i++)
            {
                if (or && i != 0)
                {
                }

                CD_InputCombination combination = command[0];
                output[i] = CreateCombinationDisplays(combination);
            }
        }

        protected virtual CD_DisplayInputsCombination CreateCombinationDisplays(CD_InputCombination combination)
        {
            CD_DisplayInputsCombination display = Instantiate(combinationLayout, combinationsParents)
                .GetComponent<CD_DisplayInputsCombination>();

            for (int j = 0; j < combination.Length; j++)
            {
                if (j != 0)
                    Or(combinationsParents);

                display.Fill(combination, this);
            }

            return display;
        }

        public virtual void And(Transform root)
        {
            if (and)
                Instantiate(ControlsDiplaySystem.Data.and, root);
        }

        public virtual void Or(Transform root)
        {
            if (or)
                Instantiate(ControlsDiplaySystem.Data.or, root);
        }


        public void Clear()
        {
            if (instantiatedCombinations != null)
            {
                for (int i = 0; i < instantiatedCombinations.Length; i++)
                {
                    instantiatedCombinations[i].Clear();
                    Destroy(instantiatedCombinations[i].gameObject);
                }

                instantiatedCombinations = null;
            }
        }

        private void OnDestroy()
        {
            Clear();
        }
    }
}
