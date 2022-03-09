using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aokoro.UI.ControlsDiplaySystem
{
    public abstract class UIItem : MonoBehaviour
    {
        protected virtual void OnEnable() => GameUIManager.Instance.OnUpdate += OnUpdate;
        protected virtual void OnDisable() => GameUIManager.Instance.OnUpdate -= OnUpdate;

        protected abstract void OnUpdate();
    }
}