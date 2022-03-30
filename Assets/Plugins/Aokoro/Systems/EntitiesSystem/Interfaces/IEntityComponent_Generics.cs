using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aokoro.Entities
{
    public interface IUpdateEntityComponent<T> : IUpdateEntityComponent, IEntityComponent<T> where T : Entity
    { }
    public interface ILateUpdateEntityComponent<T> : ILateUpdateEntityComponent, IEntityComponent<T> where T : Entity
    { }
    public interface IFixedUpdateEntityComponent<T> : IFixedUpdateEntityComponent, IEntityComponent<T> where T : Entity
    { }

    public interface IEntityComponent<T> : IEntityComponent where T : Entity
    {
        T Manager { get; set; }
        void Initiate(T manager);
    }
}
