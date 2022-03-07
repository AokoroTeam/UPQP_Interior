using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.InputSystem;
using System;

namespace Aokoro.UIManagement.ControlsDiplaySystem
{
    [System.Serializable]
    public struct CD_Input
    {
        public bool isDefault;
        [ShowAssetPreview, AllowNesting]
        public GameObject representation;

        [SerializeField]
        private string[] matchPaths;

        public bool HasValue => representation != null && (isDefault || matchPaths != null && matchPaths.Length != 0);


        public bool MatchesPath(string path)
        {
            for (int j = 0; j < matchPaths.Length; j++)
            {
                if (matchPaths[j] == path.Trim().ToLower())
                    return true;
            }

            return false;
        }
        public static bool operator ==(CD_Input c1, CD_Input c2)
        {
            return c1.Equals(c2);
        }

        public static bool operator !=(CD_Input c1, CD_Input c2)
        {
            return !c1.Equals(c2);
        }

#if UNITY_EDITOR
        public void Validate()
        {
            for (int j = 0; j < matchPaths.Length; j++)
                matchPaths[j] = matchPaths[j].Trim().ToLower();
        }

        public override bool Equals(object obj)
        {
            return obj is CD_Input input &&
                   isDefault == input.isDefault &&
                   EqualityComparer<GameObject>.Default.Equals(representation, input.representation) &&
                   EqualityComparer<string[]>.Default.Equals(matchPaths, input.matchPaths) &&
                   HasValue == input.HasValue;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(isDefault, representation, matchPaths, HasValue);
        }
#endif
    }

    public readonly struct MatchedInput
    {
        public readonly CD_Input InputData;
        public readonly string Binding;

        public MatchedInput(CD_Input input, string binding)
        {
            this.InputData = input;
            this.Binding = binding;
        }
    }
}
