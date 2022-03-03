using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Aokoro.UIManagement.ControlsDiplaySystem.UI
{
    public class MouseKey : ControlIcon
    {
        private Image image;
        private Image Image
        {
            get
            {
                if (image == null)
                    image = GetComponent<Image>();

                return image;
            }
        }

        [SerializeField]
        private Sprite lmb;
        [SerializeField]
        private Sprite rmb;
        [SerializeField]
        private Sprite mmb;



        public override void SetupIcon(string path)
        {
            image.sprite = path switch
            {
                "LMB" => lmb,
                "RMB" => rmb,
                "MMB" => mmb,
                _ => mmb,
            };
        }
    }
}