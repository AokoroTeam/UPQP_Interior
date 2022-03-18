using Aokoro.UI.ControlsDiplaySystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UPQP.Managers;

namespace UPQP.Player.Movement
{
    [AddComponentMenu("UPQP/Player/Movement/PlayerMovementUI")]
    public class PlayerMovementUI : CD_Displayer
    {
        private void Awake()
        {
            LevelManager.OnPlayerIsCreated += OnPlayerIsCreatedCallback;
        }

        private void OnPlayerIsCreatedCallback(UPQP_Player player)
        {
            LevelManager.OnPlayerIsCreated -= OnPlayerIsCreatedCallback;
            PlayerCharacter playerCharacter = player.GetLivingComponent<PlayerCharacter>();
            playerCharacter.UI = this;
            AssignActionProvider(playerCharacter);
        }
    }
}