using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Linq;
using UnityEditor.Formats.Fbx.Exporter;

namespace Aokoro.Editor.Replacer
{
    public class ModelOptimizerWindow : EditorWindow
    {
        //Drawing

        private Dictionary<int, bool> lodsFoldout = new Dictionary<int, bool>();

        //Core
        private GameObject source;
        private LODGroup sourceLG;
        private int lodCount;
        private GameObject model;


        private string path;

        [MenuItem("Tools/Aokoro/Model Optimiser")]
        static void CreateWindow()
        {
            EditorWindow.GetWindow<ModelOptimizerWindow>("Model Optimiser");
        }


        private string exportPath;
        private void Awake()
        {
            exportPath = Application.dataPath;
        }

        private void OnEnable()
        {
            path = Application.dataPath;
        }

        private void OnGUI()
        {
            GUILayout.BeginVertical("GroupBox");
            DrawMainSettings();
            GUILayout.EndVertical();
            if (source == null)
                return;

            GUILayout.BeginVertical("GroupBox");
            DrawLODManagement();
            GUILayout.EndVertical();
        }

        private void DrawMainSettings()
        {
            //Title 
            EditorGUILayout.LabelField("Main settings", EditorStyles.boldLabel);
            AokoroGUI.DrawUILine(Color.grey, 2, 0);
            EditorGUILayout.Separator();

            //Source
            source = EditorGUILayout.ObjectField("Source", source, typeof(GameObject), true) as GameObject;
            if (source == null)
                EditorGUILayout.HelpBox("Please, select a scene object", MessageType.Warning);
            else if (AssetDatabase.Contains(source))
            {
                Debug.LogError("[Model Optimiser] You cannot select an asset");
                source = null;
            }


            //Path
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Export path", path);
            if (GUILayout.Button("Select"))
            {
                string name = source != null ? $"{source.name}" : "NewModel";
                path = EditorUtility.SaveFilePanelInProject("Select file path", name, "fbx", "", path);
            }
            EditorGUILayout.EndHorizontal();
        }
        private void DrawReduce()
        {

        }

        private void DrawLODManagement()
        {
            //Title
            EditorGUILayout.LabelField("LODs", EditorStyles.boldLabel);
            AokoroGUI.DrawUILine(Color.grey, 2, 0);
            EditorGUILayout.Separator();

            //Reference
            GUI.enabled = false;
            EditorGUILayout.ObjectField("Source", sourceLG, typeof(LODGroup), true);
            GUI.enabled = true;

            //Missing lod group
            if ((sourceLG == null || sourceLG.gameObject != source) && !source.TryGetComponent(out sourceLG))
            {
                sourceLG = null;
                if (GUILayout.Button("Add LOD Group"))
                    sourceLG = Undo.AddComponent<LODGroup>(source);
                else
                    return;
            }

            if (sourceLG.lodCount == 0)
                sourceLG.SetLODs(new LOD[] { new LOD(0, new Renderer[0]) });

            //Draw model choice
            EditorGUILayout.BeginHorizontal();
            var LODS = sourceLG.GetLODs();
            model = EditorGUILayout.ObjectField("Base model", model, typeof(GameObject), false) as GameObject;

            GUI.enabled = LODS[0].renderers.Length > 0;
            GUILayout.Button("Set as first LOD", EditorStyles.miniButtonRight);
            GUI.enabled = true;
            EditorGUILayout.EndHorizontal();

            if (model == null)
            {
                EditorGUILayout.HelpBox("Please, select a model prefab to continue", MessageType.Error);
                return;
            }

            if (PrefabUtility.GetPrefabAssetType(model) != PrefabAssetType.Model)
            {
                EditorGUILayout.HelpBox("The selected prefab isn't a model prefab", MessageType.Error);
                return;
            }

            //LODS
            lodCount = EditorGUILayout.IntSlider("LOD quantity", lodCount, 1, 4);
            EditorGUILayout.Separator();
            EditorGUILayout.LabelField("Setup", EditorStyles.miniBoldLabel);
            AokoroGUI.DrawUILine(Color.gray, 1);
            for (int i = 0; i < lodCount; i++)
                DrawLODSettings(i);
        }

        private void DrawLODSettings(int lodIndex)
        {
            if (!lodsFoldout.TryGetValue(lodIndex, out bool foldout))
                lodsFoldout.Add(lodIndex, false);
            EditorGUILayout.Separator();
            foldout = EditorGUILayout.BeginFoldoutHeaderGroup(foldout, $"LOD {lodIndex}");
            lodsFoldout[lodIndex] = foldout;
            if (foldout)
            {
                EditorGUILayout.HelpBox("Not implemented yet", MessageType.Warning);

            }
            EditorGUILayout.EndFoldoutHeaderGroup();
        }

        private static void SetupLODGroup(LODGroup lODGroup)
        {
            LOD[] lods = lODGroup.GetLODs();

            for (int i = 0; i < lODGroup.lodCount; i++)
            {
                LOD lod = lods[i];
                Renderer[] renderersArray = lod.renderers;

                //Create a new parent for the export
                Transform parent = null;

                for (int j = 0; j < renderersArray.Length; j++)
                {
                    var renderer = renderersArray[j];
                    string newName = GetLODNewName(lODGroup, i, j);

                    string lodGroupName = $"LOD_{i}";
                    if (parent == null)
                    {
                        if (renderer.transform.parent.name != lodGroupName)
                            parent = new GameObject(lodGroupName).transform;
                        else
                            parent = renderer.transform.parent;

                        parent.SetParent(lODGroup.transform);
                        parent.localPosition = Vector3.zero;
                    }

                    renderer.transform.SetParent(parent);
                    renderer.transform.SetAsLastSibling();

                    renderer.gameObject.name = newName;
                }
            }
        }

        private static void ExportModel(LODGroup group, string exportPath)
        {
            GameObject gameObject = group.gameObject;
            var renderers = group.GetLODs().SelectMany(lod => lod.renderers);

            string filePath = Path.Combine(Application.dataPath, exportPath, $"{gameObject.name}.fbx");
            string exportedPath = ModelExporter.ExportObjects(filePath, renderers.Select(ctx => ctx.gameObject).ToArray());

            //If export is successful
            if (!string.IsNullOrEmpty(exportedPath))
            {
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                if (exportedPath.StartsWith(Application.dataPath))
                    exportedPath = "Assets" + exportedPath.Substring(Application.dataPath.Length);

                UnityEngine.Object[] meshes = AssetDatabase.LoadAllAssetRepresentationsAtPath(exportedPath) as UnityEngine.Object[];

                //Reassign meshes
                for (int i = 0; i < meshes.Length; i++)
                {
                    if (meshes[i] is Mesh mesh)
                    {
                        var renderer = renderers.FirstOrDefault(ctx => ctx.gameObject.name == mesh.name);
                        if (renderer != default)
                            renderer.GetComponent<MeshFilter>().mesh = mesh;
                    }
                }
            }

        }
        private static string GetLODNewName(LODGroup lODGroup, int parentIndex, int siblingIndex) => $"{lODGroup.gameObject.name}_{siblingIndex}_LOD_{parentIndex}";
    }
}
