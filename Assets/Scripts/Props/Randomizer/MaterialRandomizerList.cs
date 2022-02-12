using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UPQP.Environnement.Props
{
    [CreateAssetMenu(menuName = "UPQP/Props/Book Covers")]
    public class MaterialRandomizerList : ScriptableObject
    {
        public Material[] materials;
    }
}