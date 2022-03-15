using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Aokoro.UI.ControlsDiplaySystem.UI
{
    public class KeyboardAxis : ControlIcon
    {
        [SerializeField]
        TextMeshProUGUI[] texts;

        public override void SetupIcon(string path)
        {
            string[] textsStrings = path.Split('/');
            for (int i = 0; i < texts.Length; i++)
                texts[i].SetText(textsStrings[i]);
        }
    }
}