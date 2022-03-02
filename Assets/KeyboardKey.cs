using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Aokoro.UIManagement.ControlDisplay.UI
{
    public class KeyboardKey : ControlIcon
    {
        [SerializeField]
        TextMeshProUGUI text;

        public override void SetupIcon(string path)
        {
            text.SetText(path);
        }

    }
}