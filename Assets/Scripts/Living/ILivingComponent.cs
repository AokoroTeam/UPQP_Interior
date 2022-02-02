using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UPQP
{
    public interface ILivingComponent<T> : ILivingComponent where T : LivingBehaviour
    {
        T Manager { get; set; }
        void Initiate(T manager);
    }

    public interface ILivingComponent
    {
        bool HasUpdate { get; }
        void UpdateComponent();
        bool HasFixedUpdate { get; }
        void FixedUpdateComponent();
        bool HasLateUpdate { get; }
        void LateUpdateComponent();
    }
}
