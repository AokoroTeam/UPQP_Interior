using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UPQP.Managers;

namespace UPQP.Features.SliceView
{
    [CreateAssetMenu(fileName = "SliceView Data", menuName = "Aokoro/UPQP/SliceView/Data")]
    public class SliceView_Data : FeatureData<SliceView>
    {
        public GameObject PlayerComponent;
        public GameObject Manager;
        public GameObject UI;

        public override SliceView GenerateFeatureFromData(LevelManager controller)
        {
            SliceView sliceView = new SliceView(PlayerComponent, Manager, UI);
            sliceView.Setup(controller);

            return sliceView;
        }
    }
}