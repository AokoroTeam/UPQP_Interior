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
    [AddComponentMenu("UPQP/Managers/GameManager")]
    public class GameManager : Singleton<GameManager>
    {
        public GameUIManager mainUI;
        

        protected override void OnExistingInstanceFound(GameManager existingInstance)
        {
            Destroy(gameObject);
        }
    }
}
