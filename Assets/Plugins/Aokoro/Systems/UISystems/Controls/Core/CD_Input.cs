using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.InputSystem;

namespace Aokoro.UIManagement.ControlsDiplaySystem
{
    [System.Serializable]
    public struct CD_Input
    {
        public bool isDefault;
        [ShowAssetPreview, AllowNesting]
        public GameObject representation;

        public string[] matchPaths;

        public bool HasValue => representation != null && matchPaths != null && matchPaths.Length != 0;

    }
}
