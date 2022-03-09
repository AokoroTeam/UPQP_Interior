using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aokoro.UI
{
    [ExecuteInEditMode, DefaultExecutionOrder(-90)]
    public class GameUIManager : Singleton<GameUIManager>
    {
        public Action OnUpdate;
        public Transform WindowsParent;

        protected override void OnExistingInstanceFound(GameUIManager existingInstance)
        {
            Destroy(gameObject);
        }

        private void Update()
        {
            OnUpdate?.Invoke();
        }
    }
}