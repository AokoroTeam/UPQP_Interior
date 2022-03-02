using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aokoro.UIManagement.ControlDisplay
{
    public abstract class UIItem : MonoBehaviour
    {
        protected virtual void OnEnable() => UIManager.Instance.OnUpdate += OnUpdate;
        protected virtual void OnDisable() => UIManager.Instance.OnUpdate -= OnUpdate;

        protected abstract void OnUpdate();
    }
}