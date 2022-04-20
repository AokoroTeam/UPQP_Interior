using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aokoro.AI.Crouds
{
    public enum SkinBodyPart
    {
        Head,
        Hairs,
        Eyes,
        Body,
        Clothes,
        Pants,
        Shirt,
        Legs,
        Shoes,
        Arms,
        Hands,
    }

    [System.Serializable]
    public class SkinPart
    {
        [SerializeField]
        SkinBodyPart part;
        [SerializeField]
        private GameObject[] renderers;

        public void Randomize()
        {
            int length = renderers.Length;
            int rng = Random.Range(0, length);

            for (int i = 0; i < length; i++)
                renderers[i].SetActive(i == rng);
        }
    }
}