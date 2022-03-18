
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
            LOD[] LODs = group.GetLODs();


            int lenght = LODs.Select(ctx => ctx.renderers.Min(r => r.sharedMaterials.Length)).Min();
            Material[] set = new Material[lenght];

            for (int i = 0; i < lenght; i++)
            {
                int rng = Random.Range(0, data.materials.Length);
                set[i] = data.materials[rng];
            }

            foreach (LOD lod in LODs)
            {
                foreach (Renderer renderer in lod.renderers)
                    renderer.sharedMaterials = set;
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