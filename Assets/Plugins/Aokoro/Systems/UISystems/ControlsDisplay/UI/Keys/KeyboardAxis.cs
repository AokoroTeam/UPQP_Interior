using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

using TMPro;
using UnityEngine.UI;

namespace Aokoro.UI.ControlsDiplaySystem.UI
{
    public class KeyboardAxis : ControlIcon
    {
        [SerializeField]
        TextMeshProUGUI[] texts;
        [SerializeField]
        Image[] images;

        [SerializeField]
        Sprite upArrow;
        [SerializeField]
        Sprite downArrow;
        [SerializeField]
        Sprite rightArrow;
        [SerializeField]
        Sprite leftArrow;

        public override void SetupIcon(string path)
        {
            string[] textsStrings = path.Split('/');
            for (int i = 0; i < texts.Length; i++)
            {
                string controlPath = textsStrings[i];
                switch (controlPath.ToLower().Trim())
                {
                    case "uparrow":
                        SetArrowIcon(i, upArrow);
                        break;
                    case "downarrow":
                        SetArrowIcon(i, downArrow);
                        break;
                    case "rightarrow":
                        SetArrowIcon(i, rightArrow);
                        break;
                    case "leftarrow":
                        SetArrowIcon(i, leftArrow);
                        break;
                    default:
                        var control = InputControlPath.TryFindChild(Keyboard.current, controlPath);
                        if (control != null)
                        {
                            texts[i].SetText(control.displayName);
                            //Debug.Log(Keyboard.current[controlPath]);
                        }
                        else
                            texts[i].SetText(controlPath);

                        images[i].gameObject.SetActive(false);
                        break;
                }


            }

            void SetArrowIcon(int i, Sprite sprite)
            {
                images[i].gameObject.SetActive(true);
                texts[i].gameObject.SetActive(false);
                images[i].sprite = sprite;
            }
        }
    }
}