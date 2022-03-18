using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ArrowsRepresentation : MonoBehaviour
{
    public InputAction[] arrowsActions;
    public Image[] arrowsImages;

    public Color pressedColor;
    public Color unpressedColor;

    private void Start()
    {
        for (int i = 0; i < arrowsActions.Length; i++)
        {
            Image image = arrowsImages[i];
            image.color = unpressedColor;

            InputAction inputAction = arrowsActions[i];
            inputAction.Enable();

            inputAction.performed += ctx => OnPerformed(image);

            inputAction.canceled += ctx => OnCanceled(image);

        }
    }

    private void OnPerformed(Image image)
    {
        image.color = pressedColor;
        image.transform.localScale = Vector3.one * 1.3f;
    }

    private void OnCanceled(Image image)
    {
        image.color = unpressedColor;
        image.transform.localScale = Vector3.one;
    }

}
