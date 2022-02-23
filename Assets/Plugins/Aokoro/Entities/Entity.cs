using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using NaughtyAttributes;
using UnityEngine.AI;
using Aokoro.Tools;

namespace Aokoro.Entities
{
    public abstract class Entity : MonoBehaviour
    {
        IUpdateEntityComponent[] Ucomponents;
        ILateUpdateEntityComponent[] LUcomponents;
        IFixedUpdateEntityComponent[] FUcomponents;

        IEntityComponent[] components;

        public ComplexeProperty<bool> Freezed;

        protected virtual void Awake()
        {
            Freezed = new ComplexeProperty<bool>(false);
            Initiate<Entity>();
        }

        protected virtual void Initiate<T>() where T : Entity
        {

            components = SetupComponents<T>(GetComponentsInChildren<IEntityComponent>());

            List<IUpdateEntityComponent> updatesList = new();
            List<ILateUpdateEntityComponent> lateUpdatesList = new();
            List<IFixedUpdateEntityComponent> fixedUpdatesList = new();

            for (int i = 0; i < components.Length; i++)
            {
                IEntityComponent component = components[i];
                if (component is IUpdateEntityComponent u)
                    updatesList.Add(u);
                if (component is IFixedUpdateEntityComponent fu)
                    fixedUpdatesList.Add(fu);
                if (component is ILateUpdateEntityComponent lu)
                    lateUpdatesList.Add(lu);
            }

        }

        private IEntityComponent[] SetupComponents<T>(IEntityComponent[] ChildComponents) where T : Entity
        {
            Type targetInterface = typeof(IEntityComponent<T>);

            List<IEntityComponent> componentsList = new List<IEntityComponent>();
            int count = ChildComponents.Length;

            for (int i = 0; i < count; i++)
            {
                IEntityComponent component = ChildComponents[i];
                Type componentType = component.GetType();
                Type[] implementedInterfaces = componentType.GetInterfaces();

                for (int j = 0; j < implementedInterfaces.Length; j++)
                {
                    Type interfaceType = implementedInterfaces[j];
                    if (interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == typeof(IEntityComponent<>))
                    {
                        var ga = interfaceType.GetGenericArguments()[0];
                        if (ga == typeof(T) || ga.IsSubclassOf(typeof(T)))
                        {
                            var c = (component as IEntityComponent<T>);

                            c.Manager = this as T;
                            c.Initiate(this as T);
                            break;
                        }
                        else
                        {
                            Debug.LogError("Wrong manager for this living component");
                        }
                    }

                }

                componentsList.Add(component);
            }

            return componentsList.ToArray();
        }

        protected virtual void Update()
        {
            if (!Freezed.Output)
            {
                for (int i = 0; i < Ucomponents.Length; i++)
                {
                    try
                    {
                        Ucomponents[i].UpdateComponent();
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e, Ucomponents[i] as MonoBehaviour);
                    }
                }
            }
        }

        protected virtual void FixedUpdate()
        {
            if (!Freezed.Output)
            {
                for (int i = 0; i < FUcomponents.Length; i++)
                {
                    try
                    {
                        FUcomponents[i].FixedUpdateComponent();
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e, FUcomponents[i] as MonoBehaviour);
                    }
                }
            }
        }

        protected virtual void LateUpdate()
        {
            if (!Freezed.Output)
            {
                for (int i = 0; i < LUcomponents.Length; i++)
                {
                    try
                    {
                        LUcomponents[i].LateUpdateComponent();
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e, LUcomponents[i] as MonoBehaviour);
                    }
                }
            }
        }


        public bool GetLivingComponent<T>(out T component) where T : IEntityComponent
        {
            component = default;
            for (int i = 0; i < components.Length; i++)
            {
                if (components[i] is T c)
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
