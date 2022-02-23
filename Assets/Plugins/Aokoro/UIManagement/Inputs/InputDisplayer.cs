using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Aokoro.UIManagement.Controls
{
    public class ControlsDisplayer : MonoBehaviour
    {
        [SerializeField]
        string text;

        [SerializeField]
        TextMeshProUGUI description;

        private void Awake()
        {

        }

        private void Setup()
        {
            description.text = text;

        }
    }
}
