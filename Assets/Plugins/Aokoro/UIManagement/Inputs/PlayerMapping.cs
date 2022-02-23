using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Aokoro.Entities;
using UPQP;

namespace Aokoro.UIManagement.Controls
{
    public class PlayerMapping : MonoBehaviour, IEntityComponent<UPQP.PlayerManager>
    {
        public PlayerManager Manager { get; set; }

        public bool HasUpdate => false;

        public bool HasFixedUpdate => false;

        public bool HasLateUpdate => false;

        public void FixedUpdateComponent()
        {
        }

        public void LateUpdateComponent()
        {
        }

        public void Initiate(PlayerManager manager)
        {
            throw new System.NotImplementedException();
        }

        public void UpdateComponent()
        {
            throw new System.NotImplementedException();
        }
    }
}