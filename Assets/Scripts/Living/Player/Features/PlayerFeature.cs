using Aokoro.Entities.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UPQP.Features;

namespace UPQP.Features
{

    public abstract class PlayerFeature : MonoBehaviour, IPlayerInputAssetProvider
    {
        [SerializeField] string featureName;
        [SerializeField] private InputActionAsset actions;
        public InputActionAsset Actions { get => actions; set => actions = value; }

        public abstract void ExecuteFeature(PlayerManager player);
        public abstract void EndFeature(PlayerManager player);
    }
}