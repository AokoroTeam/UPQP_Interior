using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Aokoro.UI.ControlsDiplaySystem.UI
{
    public class KeyboardKey : ControlIcon
    {
        [SerializeField]
        TextMeshProUGUI text;

        public override void SetupIcon(string path)
        {
            text.SetText(path switch
            {
                "&" => "1",
                "é" => "2",
                "\"" => "3",
                "'" => "4",
                "(" => "5",
                "-" => "6",
                "è" => "7",
                "_" => "8",
                "ç" => "9",
                "à" => "0",
                _ => path,
            });
        }
    }
}