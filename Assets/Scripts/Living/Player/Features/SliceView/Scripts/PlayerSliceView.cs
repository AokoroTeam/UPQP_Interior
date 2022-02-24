using Aokoro.Entities.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UPQP.Features;

namespace UPQP.SliceView
{

    public class PlayerSliceView : PlayerFeature
    {
        public override void ExecuteFeature(PlayerManager player)
        {
            SliceViewManager.Instance.EnterSliceView();
            player.Freezed.Subscribe(this, 20, true);
        }

        public override void EndFeature(PlayerManager player)
        {
            SliceViewManager.Instance.ExitSliceView();
            player.Freezed.Unsubscribe(this);
        }
    }
}