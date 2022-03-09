using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UPQP.Features;
using Aokoro.UI;
using Michsky.UI.ModernUIPack;
using Aokoro;

namespace UPQP.Managers
{
    [DefaultExecutionOrder(-100)]
    public class GameManager : Singleton<GameManager>
    {
        public GameUIManager mainUI;
        public WindowManager WindowManager { get; private set; }

        protected override void OnExistingInstanceFound(GameManager existingInstance)
        {
            Destroy(gameObject);
        }

        protected override void Awake()
        {
            //Application.targetFrameRate = 30;
            WindowManager = mainUI.GetComponent<WindowManager>();

        }

    }
}
