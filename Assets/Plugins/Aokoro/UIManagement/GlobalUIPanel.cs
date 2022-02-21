using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalUIPanel : MonoBehaviour
{
    public string PanelName;

    public virtual void Hide(string newPanel)
    {
        gameObject.SetActive(false);
    }

    public virtual void Show(string oldPanel)
    {
        gameObject.SetActive(true);
    }

}
