#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Formats.Fbx.Exporter;
using System.IO;
using UnityEditor;
using NaughtyAttributes;

namespace Aokoro.Tools.LODs
{
    public class LODHelper : MonoBehaviour
    {
        public string exportPath;

        [Button("Export", EButtonEnableMode.Editor)]
        public void ExportGameObjects()
        {
            LODGroup lODGroup;
            if (!gameObject.TryGetComponent(out lODGroup))
                lODGroup = gameObject.AddComponent<LODGroup>();

            var lods = lODGroup.GetLODs();
            List<Renderer> renderers = new List<Renderer>();
            List<GameObject> objectsToExport = new List<GameObject>();

            //Sorting lods and renaming them
            for (int i = 0; i < lODGroup.lodCount; i++)
            {
                LOD lod = lods[i];
                Renderer[] renderersArray = lod.renderers;

                //Create a new parent for the export
                Transform parent = null;

                for (int j = 0; j < renderersArray.Length; j++)
                {
                    var renderer = renderersArray[j];
                    renderers.Add(renderer);
                    objectsToExport.Add(renderer.gameObject);
                    string newName = $"{gameObject.name}_{j}_LOD_{i}";
                    /*
                    string lodGroupName = $"LOD_{i}";
                    if (renderer.transform.parent.name != lodGroupName)
                    {
                        if (parent == null)
                        {
                            parent = new GameObject(lodGroupName).transform;
                            parent.SetParent(transform);
                            parent.localPosition = Vector3.zero;
                        }
                    }
                    renderer.transform.SetParent(parent);
                    renderer.transform.SetAsLastSibling();
*/
                    renderer.gameObject.name = newName;
                }
            }
            string filePath = Path.Combine(Application.dataPath, $"{gameObject.name}.fbx");
            string exportedPath = ModelExporter.ExportObjects(filePath, objectsToExport.ToArray());

            //If export is successful
            if (!string.IsNullOrEmpty(exportedPath))
            {
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                if (exportedPath.StartsWith(Application.dataPath))
                    exportedPath = "Assets" + exportedPath.Substring(Application.dataPath.Length);

                Object[] meshes = AssetDatabase.LoadAllAssetRepresentationsAtPath(exportedPath) as Object[];

                //Reassign meshes
                for (int i = 0; i < meshes.Length; i++)
                {
                    if (meshes[i] is Mesh mesh)
                    {

                        int index = renderers.FindIndex(ctx => ctx.gameObject.name == mesh.name);
                        if (index != -1)
                        {
                            renderers[index].GetComponent<MeshFilter>().mesh = mesh;
                            renderers.RemoveAt(index);
                        }
                    }
                }
            }
        }

    }
}
#endif