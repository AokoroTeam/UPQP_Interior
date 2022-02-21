using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Aokoro.AI.Crouds
{
    public class CroudSkin : MonoBehaviour
    {
        [HideInInspector]
        public Animator animator;

        [SerializeField]
        private SkinPart[] skinParts;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public void RandomizeSkin()
        {
            for (int i = 0; i < skinParts.Length; i++)
                skinParts[i].Randomize();
        }

        public void Foot_R()
        {

        }

        public void Foot_L()
        {

        }
    }
}