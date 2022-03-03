using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

namespace Aokoro.UIManagement.ControlsDiplaySystem
{
    public class CD_DisplayInputsCombination : MonoBehaviour
    {
        private Transform root;
        private GameObject[] instantiatedInputs;
        public virtual void Fill(CD_InputCombination combination, CD_DisplayCommand commandDisplayer)
        {
            int length = combination.Length;
            instantiatedInputs = new GameObject[length];
            for (int i = 0; i < length; i++)
            {
                if (i != 0)
                    commandDisplayer.And(root);

                var input = combination[i];
                instantiatedInputs[i] = GameObject.Instantiate(input.representation, root);
            }
        }
        public void Clear()
        {
            if (instantiatedInputs == null)
                return;

            for (int i = 0; i < instantiatedInputs.Length; i++)
                Destroy(instantiatedInputs[i].gameObject);

            instantiatedInputs = null;
        }

        private void OnDestroy()
        {
            Clear();
        }
    }
}
