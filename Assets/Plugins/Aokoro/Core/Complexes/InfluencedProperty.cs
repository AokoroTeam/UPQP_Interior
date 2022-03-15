﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Aokoro
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

    public struct Influence<T>
    {
        public int Priority { get; private set; }
        public T Value { get; private set; }

        public Influence(int Priority, T Value)
        {
            this.Priority = Priority;
            this.Value = Value;
        }
        public Influence(int Priority) : this(Priority, default) { }
        public Influence(PriorityTags Priority, T Value) : this((int)Priority, Value) { }
        public Influence(PriorityTags Priority) : this((int)Priority, default) { }

        public void Set(T Value) => this.Value = Value;
    }
    [System.Serializable]
    public class InfluencedProperty<T> 
    {
        public event Action<T, object> OnValueChanged;
        private Dictionary<object, Influence<T>> influences = new Dictionary<object, Influence<T>>();

        private bool influencesListIsDirty;

        private readonly T defaultValue;

        private object _mainInfluencer;
        
        private object MainInfluencer
        {
            get
            {
                if (influencesListIsDirty)
                    SetMainInfluenceur();

                return _mainInfluencer;
            }
                
            set => _mainInfluencer = value;
        }

        public T Value
        {
            get
            {
                if (influencesListIsDirty)
                    SetMainInfluenceur();

                return MainInfluencer == null ? defaultValue : influences [MainInfluencer].Value;
            }
        }
        
        public InfluencedProperty() : this(default) { }
        
        public InfluencedProperty(T defaultValue)
        {
            this.influencesListIsDirty = true;
            influences = new Dictionary<object, Influence<T>>();
            this.defaultValue = defaultValue;
        }

        public T GetValueFromInfluencer(object influencer) => influences.TryGetValue(influencer, out Influence<T> i) ? i.Value : defaultValue;

        public void Lock(object influencer, int priority) => Subscribe(influencer, priority, Value);
        public void Lock(object influencer, PriorityTags priority) => Lock(influencer, (int)priority);
        public void Subscribe(object influencer, int priority) => Subscribe(influencer, priority, defaultValue);
        public void Subscribe(object influencer, PriorityTags priority) => Subscribe(influencer, (int)priority, defaultValue);
        public void Subscribe(object influencer, int priority, T Value)
        {
            Influence<T> i = new Influence<T>(priority, Value);

            if (!influences.ContainsKey(influencer))
                influences.Add(influencer, i);
            else
                influences[influencer] = i;

            this.influencesListIsDirty = true;
        }
        
        public void Subscribe(object influencer, PriorityTags priority, T Value) => Subscribe(influencer, (int)priority, Value);

        public void Unsubscribe(object influencer)
        {
            if (IsInfluencing(influencer))
            {
                influences.Remove(influencer);
                this.influencesListIsDirty = true;
            }
        }
        
        public void Unsubscribe(object influencer, object transferTo) => Unsubscribe(influencer, transferTo, GetValueFromInfluencer(influencer));

        public void Unsubscribe(object influencer, object transferTo, T value)
        {
            if (IsPriority(influencer))
                Set(transferTo, value);

            Unsubscribe(influencer);
        }

        public void Set(object influencer, T value)
        {
            if (influences.ContainsKey(influencer))
            {
                var i = influences[influencer];
                i.Set(value);
                influences[influencer] = i;
            }
        }

        public bool IsPriority(object influencer) => influencer != null && MainInfluencer == influencer;
        public bool IsInfluencing(object influencer) => influences.ContainsKey(influencer);

        private void SetMainInfluenceur()
        {
            this.influencesListIsDirty = false;
            MainInfluencer = null;

            int lastPriority = int.MinValue;
            foreach (var kv in influences)
            {
                if (kv.Value.Priority > lastPriority)
                    MainInfluencer = kv.Key;
            }
        }

        public static implicit operator T(InfluencedProperty<T> interaction) => interaction.Value;
    }
}


