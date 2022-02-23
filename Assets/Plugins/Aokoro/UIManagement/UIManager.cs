using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aokoro.UIManagement.Controls
{
    [ExecuteInEditMode]
    public class UIManager : Singleton<UIManager>
    {
        public Action OnUpdate;

        protected override void OnExistingInstanceFound(UIManager existingInstance)
        {
            Destroy(gameObject);
        }

        private void Update()
        {
            OnUpdate?.Invoke();
        }
    }
}