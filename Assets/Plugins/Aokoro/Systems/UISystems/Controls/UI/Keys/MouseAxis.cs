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
        private GameObject horizontal1;
        [SerializeField]
        private GameObject horizontal2;
        [SerializeField]
        private GameObject vertical1;
        [SerializeField]
        private GameObject vertical2;


        public override void SetupIcon(string path)
        {
            switch (path)
            {
                case ("delta"):
                    horizontal1.SetActive(true);
                    horizontal2.SetActive(true);
                    vertical1.SetActive(true);
                    vertical2.SetActive(true);
                    break;
                case ("delta x"):
                    horizontal1.SetActive(true);
                    horizontal2.SetActive(true);
                    vertical1.SetActive(false);
                    vertical2.SetActive(false);
                    break;
                case ("delta y"):
                    horizontal1.SetActive(false);
                    horizontal2.SetActive(false);
                    vertical1.SetActive(true);
                    vertical2.SetActive(true);
                    break;
            }
        }
    }
}