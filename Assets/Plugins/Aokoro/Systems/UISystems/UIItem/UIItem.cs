using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aokoro.UI.ControlsDiplaySystem
{
    public abstract class UIItem : MonoBehaviour
    {
        GameUIManager uIManager;

        private void Awake()
        {
            uIManager = GetComponentInParent<GameUIManager>();
            if (uIManager == null)
                Destroy(this);
        }
        protected virtual void OnEnable()
        {
            if (uIManager != null)
                uIManager.OnUpdate += OnUpdate;
        }
        protected virtual void OnDisable()
        {
            if (uIManager != null)
                uIManager.OnUpdate -= OnUpdate;
        }

        protected abstract void OnUpdate();
    }
}