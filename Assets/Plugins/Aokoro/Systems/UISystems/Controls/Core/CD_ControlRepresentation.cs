using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.InputSystem;

namespace Aokoro.UIManagement.ControlDisplay
{
    [System.Serializable]
    public struct CD_ControlRepresentation
    {
        public string[] matchPaths;
        [ShowAssetPreview, AllowNesting]
        public GameObject representation;
    }
}
