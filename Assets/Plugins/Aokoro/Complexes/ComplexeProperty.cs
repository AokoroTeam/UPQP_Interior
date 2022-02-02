using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Aokoro.Tools
{
    public enum PriorityTags
    {
        Free = -1,
        Smallest = 1,
        VerySmall = 3,
        Small = 5,
        Default = 10,
        High = 25,
        VeryHigh = 50,
        Highest = 100
    }

    public struct Interaction<T>
    {
        public int Priority { get; private set; }
        public T Value { get; private set; }

        public Interaction(int Priority, T Value)
        {
            this.Priority = Priority;
            this.Value = Value;
        }
        public Interaction(int Priority) : this(Priority, default) { }
        public Interaction(PriorityTags Priority, T Value) : this((int)Priority, Value) { }
        public Interaction(PriorityTags Priority) : this((int)Priority, default) { }

        public void Set(T Value) => this.Value = Value;
    }
    [System.Serializable]
    public class ComplexeProperty<T> 
    {
        private Dictionary<object, Interaction<T>> interactionsDict = new Dictionary<object, Interaction<T>>();

        private bool dirty;
        private readonly T defaultValue;

        private object outputKey;
        private object OutputKey
        {
            get
            {
                if (dirty)
                    Refresh();

                return outputKey;
            }
                
            set => outputKey = value;
        }
        public T Output => OutputKey == null ? defaultValue : interactionsDict[OutputKey].Value;
        
        public int InteractionsCount => interactionsDict.Count;
        public ComplexeProperty() : this(default) { }
        
        public ComplexeProperty(T defaultValue)
        {
            this.dirty = true;
            interactionsDict = new Dictionary<object, Interaction<T>>();
            this.defaultValue = defaultValue;
        }

        public T GetValue(object key) => interactionsDict.TryGetValue(key, out Interaction<T> i) ? i.Value : defaultValue;

        public void Lock(object key, int priority) => Subscribe(key, priority, Output);
        public void Lock(object key, PriorityTags priority) => Lock(key, (int)priority);
        public void Subscribe(object key, int priority) => Subscribe(key, priority, defaultValue);
        public void Subscribe(object key, PriorityTags priority) => Subscribe(key, (int)priority, defaultValue);
        public void Subscribe(object key, int priority, T Value)
        {
            Interaction<T> i = new Interaction<T>(priority, Value);

            if (!interactionsDict.ContainsKey(key))
                interactionsDict.Add(key, i);
            else
                interactionsDict[key] = i;

            this.dirty = true;
        }
        
        public void Subscribe(object key, PriorityTags priority, T Value) => Subscribe(key, (int)priority, Value);

        public void Unsubscribe(object key)
        {
            if (IsContributing(key))
            {
                interactionsDict.Remove(key);
                this.dirty = true;
            }
        }
        
        public void Unsubscribe(object key, object transferTo) => Unsubscribe(key, transferTo, GetValue(key));

        public void Unsubscribe(object key, object transferTo, T value)
        {
            if (IsPriority(key))
            {
                Set(transferTo, value);
            }
            Unsubscribe(key);
        }
        private void Set(T value)
        {
            if (OutputKey != null)
                Set(OutputKey, value);

        }
        public void Set(object key, T value)
        {
            if (interactionsDict.ContainsKey(key))
            {
                var i = interactionsDict[key];
                i.Set(value);
                interactionsDict[key] = i;
            }
        }

        public bool IsPriority(object key) => key != null && OutputKey == key;
        public bool IsContributing(object key) => interactionsDict.ContainsKey(key);

        private void Refresh()
        {
            this.dirty = false;

            if (interactionsDict.Count == 0)
            {
                OutputKey = null;
                return;
            }

            List<object> priorities = new List<object>(interactionsDict.Count);

            foreach (var kv in interactionsDict)
                priorities.Add(kv.Key);

            priorities.Sort((x, y) =>
            {
                int xPrio = interactionsDict[x].Priority;
                int yPrio = interactionsDict[y].Priority;

                return xPrio.CompareTo(yPrio);
            });

            OutputKey = priorities[priorities.Count - 1];
        }

        public static implicit operator T(ComplexeProperty<T> interaction) => interaction.Output;
    }
}


