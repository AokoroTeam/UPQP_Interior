using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aokoro.Entities
{

    public interface IEntityComponent { }

    public interface IUpdateEntityComponent : IEntityComponent
    {
        void UpdateComponent();
    }
    public interface ILateUpdateEntityComponent : IEntityComponent
    {
        void LateUpdateComponent();
    }
    public interface IFixedUpdateEntityComponent : IEntityComponent
    {
        void FixedUpdateComponent();
    }
}
