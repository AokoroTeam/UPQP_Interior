#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Formats.Fbx.Exporter;
#endif

using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using System.IO;

using NaughtyAttributes;

namespace Aokoro.Tools.LODs
{
    public class LODCombiner : MonoBehaviour
    {
        public string exportPath;

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (exportPath != null && exportPath.StartsWith("Assets/"))
                exportPath = exportPath.Remove(0, 7);
        }

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

                    string lodGroupName = $"LOD_{i}";
                    if (parent == null)
                    {
                        if (renderer.transform.parent.name != lodGroupName)
                            parent = new GameObject(lodGroupName).transform;
                        else
                            parent = renderer.transform.parent;

                        parent.SetParent(transform);
                        parent.localPosition = Vector3.zero;
                    }

                    renderer.transform.SetParent(parent);
                    renderer.transform.SetAsLastSibling();

                    renderer.gameObject.name = newName;
                }
            }
            string filePath = Path.Combine(Application.dataPath, exportPath, $"{gameObject.name}.fbx");
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
#endif

    }
}