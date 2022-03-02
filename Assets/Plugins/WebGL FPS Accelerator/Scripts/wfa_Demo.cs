﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using System.Runtime.InteropServices;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AG_WebGLFPSAccelerator
{
    public class wfa_Demo : MonoBehaviour
    {
        public static wfa_Demo instance;

#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")]
        public static extern bool isAndroid2();

        [DllImport("__Internal")]
        public static extern bool isiOS2();
#endif
        
        [HideInInspector]
        public GameObject requiredSettings;
        
        [HideInInspector]
        public GameObject postProcessVolume;
        
        private float shadowDistanceTemp;

        [HideInInspector]
        public bool isiOS;
        [HideInInspector]
        public bool isAndroid;

        void Awake()
        {
            instance = this;
        }

        void Start()
        {

#if UNITY_2021_1_OR_NEWER && USING_URP
            VolumeProfile VolumeProfile1 = Resources.Load<VolumeProfile>("URP Volume Profile 2");
            postProcessVolume.GetComponent<Volume>().profile = VolumeProfile1;
#endif

#if UNITY_WEBGL && !UNITY_EDITOR
            isiOS = isiOS2();
            isAndroid = isAndroid2();
#endif

#if UNITY_EDITOR
            EditorApplication.playModeStateChanged += playModeStateChanged;
#endif

            QualitySettings.vSyncCount = 0;
            QualitySettings.shadowDistance = 300f;

#if USING_URP
            var rpAsset = UnityEngine.Rendering.GraphicsSettings.renderPipelineAsset;
            var urpAsset = (UnityEngine.Rendering.Universal.UniversalRenderPipelineAsset)rpAsset;

            shadowDistanceTemp = urpAsset.shadowDistance;
            urpAsset.shadowDistance = 200f;
#endif

            m1();
            generateMap.instance.m1();
        }

#if UNITY_EDITOR
        public void playModeStateChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.ExitingPlayMode)
            {
#if USING_URP
                var rpAsset = UnityEngine.Rendering.GraphicsSettings.renderPipelineAsset;
                var urpAsset = (UnityEngine.Rendering.Universal.UniversalRenderPipelineAsset)rpAsset;

                urpAsset.shadowDistance = shadowDistanceTemp;
#endif
            }
        }
#endif

        public void m1()
        {
#if !UNITY_EDITOR
            if (requiredSettings)
            {
                requiredSettings.SetActive(false);
            }
#endif
        }
    }
}