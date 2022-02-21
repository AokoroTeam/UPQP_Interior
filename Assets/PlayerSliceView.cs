using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UPQP.Environnement.Features.SliceView
{

    public class PlayerSliceView : MonoBehaviour, ILivingComponent<PlayerManager>
    {
        public PlayerManager Manager { get; set; }

        public bool HasUpdate => false;

        public bool HasFixedUpdate => false;

        public bool HasLateUpdate => false;


        public void Initiate(PlayerManager manager)
        {
            //manager.playerInput.actions.FindActionMap("Enter")
        }

        public void EnterSliceView()
        {

        }

        public void FixedUpdateComponent()
        {
        }

        public void LateUpdateComponent()
        {
        }

        public void UpdateComponent()
        {
        }
    }
}