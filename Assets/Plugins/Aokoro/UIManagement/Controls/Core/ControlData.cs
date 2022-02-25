using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.InputSystem;

namespace Aokoro.UIManagement.Controls
{
    [System.Serializable]
    public struct ControlData
    {
        public string[] matchPaths;
        [ShowAssetPreview, AllowNesting]
        public GameObject representation;
    }
}
