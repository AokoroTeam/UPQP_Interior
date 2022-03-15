using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Credits : https://www.patrykgalach.com/2020/01/27/assigning-interface-in-unity-inspector/
/// </summary>
namespace Aokoro
{
    /// <summary>
    /// Attribute that require implementation of the provided interface.
    /// </summary>
    public class RequireInterfaceAttribute : PropertyAttribute
    {
        // Interface type.
        public System.Type requiredType { get; private set; }

        /// <summary>
        /// Requiring implementation of the <see cref="T:RequireInterfaceAttribute"/> interface.
        /// </summary>
        /// <param name="type">Interface type.</param>
        public RequireInterfaceAttribute(System.Type type)
        {
            this.requiredType = type;
        }
    }
}
