
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System.Linq;

namespace Aokoro.Tools.Props
{
    [RequireComponent(typeof(LODGroup))]
    public class MaterialRandomizer : MonoBehaviour
    {
        [Expandable]
        public MaterialRandomizerList data;

        [Button]
        private void Randomize()
        {
            LODGroup group = GetComponent<LODGroup>();
            LOD[] lODs = group.GetLODs();
            int bookCount = lODs[0].renderers.Length;

            for (int i = 0; i < bookCount; i++)
            {
                int rng = Random.Range(0, data.materials.Length);
                for (int j = 0; j < group.lodCount; j++)
                    lODs[j].renderers[i].material = data.materials[rng];
            }
        }

        [Button]
        private void RandomizeAllWithSameList()
        {
            var props = Object.FindObjectsOfType<MaterialRandomizer>();
            RandomizeMultiple(props.Where(p => p.data == data).ToArray());
        }


        [Button]
        private void RandomizeAll()
        {
            var props = Object.FindObjectsOfType<MaterialRandomizer>();
            RandomizeMultiple(props);
        }


        private void RandomizeMultiple(MaterialRandomizer[] array)
        {
            for (int i = 0; i < array.Length; i++)
                array[i].Randomize();
        }
    }
}