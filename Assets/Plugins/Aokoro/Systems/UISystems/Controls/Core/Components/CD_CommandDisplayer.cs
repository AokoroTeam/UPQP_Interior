using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

namespace Aokoro.UIManagement.ControlDisplay
{
    public class CD_CommandDisplayer : MonoBehaviour
    {
        [SerializeField]
        TextMeshProUGUI description;
        [SerializeField]
        private Transform controlsListParent;

        private CD_ControlRepresentation[] datas;

        public void Bind(CD_Command command)
        {
            description.text = command.actionName;
        }
    }
}
