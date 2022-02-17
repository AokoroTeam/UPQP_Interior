using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using NaughtyAttributes;
using UnityEngine.AI;
using Aokoro.Tools;

namespace UPQP
{
    public abstract class LivingBehaviour : MonoBehaviour
    {
        ILivingComponent[] components;
        Action[] updates;
        Action[] lateupdates;
        Action[] fixedUpdates;

        public ComplexeProperty<bool> Freezed;

        protected virtual void Awake()
        {
            Freezed = new ComplexeProperty<bool>(false);
            Initiate<LivingBehaviour>();
        }

        protected virtual void Initiate<T>() where T : LivingBehaviour
        {
            List<ILivingComponent> viableComponents = new List<ILivingComponent>();
            SetupComponents<T>(viableComponents, GetComponentsInChildren<ILivingComponent>());

            this.components = viableComponents.ToArray();

            updates = this.components.Where(ctx => ctx.HasUpdate).Select(ctx => new Action(ctx.UpdateComponent)).ToArray();
            lateupdates = this.components.Where(ctx => ctx.HasLateUpdate).Select(ctx => new Action(ctx.LateUpdateComponent)).ToArray();
            fixedUpdates = this.components.Where(ctx => ctx.HasFixedUpdate).Select(ctx => new Action(ctx.FixedUpdateComponent)).ToArray();

        }

        private void SetupComponents<T>(List<ILivingComponent> viableComponents, ILivingComponent[] components) where T : LivingBehaviour
        {
            int count = components.Length;

            for (int i = 0; i < count; i++)
            {
                ILivingComponent component = components[i];
                try
                {
                    Type type = component.GetType();
                    if (component is ILivingComponent<T> e)
                    {
                        T m = this as T;
                        e.Manager = m;
                        e.Initiate(m);
                    }
                    else if (!type.IsGenericType)
                    {
                        Debug.Log("Destroying uncompatible Component");
                        Destroy(component as MonoBehaviour);
                        continue;
                    }

                    viableComponents.Add(component);
                }
                catch (Exception e)
                {
                    MonoBehaviour mono = component as MonoBehaviour;
                    Debug.Log("Error on " + mono.name, mono);
                    throw e;
                }
            }
        }

        protected virtual void Update() 
        {
            if (!Freezed.Output) 
                CallComponentsUpdates(updates); 
        }

        protected virtual void FixedUpdate() 
        { 
            if (!Freezed.Output)
                CallComponentsUpdates(fixedUpdates);
        } 

        protected virtual void LateUpdate() 
        {
            if (!Freezed.Output) 
                CallComponentsUpdates(lateupdates); 
        }

        private void CallComponentsUpdates(Action[] methods)
        {
            int length = methods.Length;
            for (int i = 0; i < length; i++)
            {
                methods[i]?.Invoke();
            }
        }

        public bool GetLivingComponent<T>(out T component) where T : ILivingComponent
        {
            component = default;
            for (int i = 0; i < components.Length; i++)
            {
                if(components[i] is T c)
                {
                    component = c;
                    return true;
                }
            }

            return false;
        }

        public void Log(string message)
        {
            Debug.Log(string.Concat("[", gameObject.name, "] => ", message));
        }


    }
}
