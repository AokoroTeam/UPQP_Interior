using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Aokoro.UIManagement.ControlsDiplaySystem.UI
{
    public class MouseAxis : ControlIcon
    {

        [SerializeField]
        private GameObject horizontal;
        [SerializeField]
        private GameObject vertical;


        public override void SetupIcon(string path)
        {
            switch (path)
            {
                case ("delta"):
                    horizontal.SetActive(true);
                    vertical.SetActive(true);
                    break;
                case ("delta/x"):
                    horizontal.SetActive(false);
                    vertical.SetActive(true);
                    break;
                case ("delta/y"):
                    horizontal.SetActive(true);
                    vertical.SetActive(false);
                    break;
            }
        }
    }
}