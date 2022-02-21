using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UPQP.Features;

namespace UPQP.SliceView
{

    public class PlayerSliceView : PlayerFeature
    {
        public override void EnterFeature(PlayerManager player)
        {
            SliceViewManager.Instance.EnterSliceView();
            player.Freezed.Subscribe(this, 20, true);
        }

        public override void ExitFeature(PlayerManager player)
        {
            SliceViewManager.Instance.ExitSliceView();
            player.Freezed.Unsubscribe(this);
        }
    }
}