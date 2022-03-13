using Michsky.UI.ModernUIPack;
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

        public static WindowManager WindowManager { get; private set; }

        protected override void OnExistingInstanceFound(GameUIManager existingInstance)
        {
            Destroy(gameObject);
        }
        protected override void Awake()
        {
            base.Awake();
            if(IsInstance)
            {
                WindowManager = GetComponent<WindowManager>();

            }
        }



        private void Update()
        {
            OnUpdate?.Invoke();
        }

    }
}