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
            components = SetupComponents<T>(GetComponentsInChildren<ILivingComponent>());

            List<Action> updatesList = new List<Action>();
            List<Action> lateUpdatesList = new List<Action>();
            List<Action> fixedUpdatesList = new List<Action>();

            for (int i = 0; i < components.Length; i++)
            {
                ILivingComponent component = components[i];
                if (component.HasUpdate)
                    updatesList.Add(new Action(component.UpdateComponent));
                if (component.HasLateUpdate)
                    updatesList.Add(new Action(component.LateUpdateComponent));
                if (component.HasFixedUpdate)
                    updatesList.Add(new Action(component.FixedUpdateComponent));
            }

            updates = updatesList.ToArray();
            lateupdates = lateUpdatesList.ToArray();
            fixedUpdates = fixedUpdatesList.ToArray();
        }

        private ILivingComponent[] SetupComponents<T>(ILivingComponent[] components) where T : LivingBehaviour
        {
            List<ILivingComponent> componentsList = new List<ILivingComponent>();
            int count = components.Length;

            for (int i = 0; i < count; i++)
            {
                ILivingComponent component = components[i];
                try
                {
                    Type[] interfaces = component.GetType().GetInterfaces();

                    for (int j = 0; j < interfaces.Length; j++)
                    {
                        Type _interface = interfaces[j].GetGenericTypeDefinition();
                        //Specific
                        if (_interface == typeof(ILivingComponent<T>))
                        {
                            T m = this as T;
                            var c = (component as ILivingComponent<T>);

                            c.Manager = m;
                            c.Initiate(this as T);
                        }

                    }
                }
                catch (Exception e)
                {
                    MonoBehaviour mono = component as MonoBehaviour;
                    Debug.Log("Error on " + mono.name, mono);
                    throw e;
                }

                componentsList.Add(component);
            }

            return componentsList.ToArray();
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
