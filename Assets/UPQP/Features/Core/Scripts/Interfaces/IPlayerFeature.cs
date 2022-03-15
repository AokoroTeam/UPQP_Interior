using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UPQP.Player;

namespace UPQP.Features
{

    public interface IPlayerFeature
    {
        public Feature @Feature { get; }
        public string MapName { get; }
        public UPQP_Player Player { get; set; }
    }
}