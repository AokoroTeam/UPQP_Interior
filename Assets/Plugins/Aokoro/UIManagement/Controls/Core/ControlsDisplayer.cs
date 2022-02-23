using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

namespace Aokoro.UIManagement.Controls
{
    public class ControlsDisplayer : MonoBehaviour
    {

        [SerializeField]
        string text;

        [SerializeField]
        TextMeshProUGUI description;

        [SerializeField]
        private Transform controlsListParent;


        public void Setup(GameObject[] allControls)
        {
            description.text = text;

        }


    }
}
