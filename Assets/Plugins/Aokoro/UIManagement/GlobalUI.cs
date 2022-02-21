using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace Aokoro.UIManagement
{
    [ExecuteInEditMode]
    public class GlobalUI : Singleton<GlobalUI>
    {
        [SerializeField]
        private string defaultPanel;
        [SerializeField, ReadOnly]
        private string currentPanel;

        private Dictionary<string, GlobalUIPanel> panels;

        protected override void OnExistingInstanceFound(GlobalUI existingInstance)
        {
            Destroy(gameObject);
        }

        private void OnTransformChildrenChanged()
        {
            Awake();
        }

        protected override void Awake()
        {
            base.Awake();
            panels = new Dictionary<string, GlobalUIPanel>();
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);
                if (!child.TryGetComponent(out GlobalUIPanel panel))
                {
                    panel = child.gameObject.AddComponent<GlobalUIPanel>();
                    panel.PanelName = $"Panel {i}";
                    if (!panels.TryAdd(panel.PanelName, panel))
                    {
                        Debug.LogError($"A panel with the name {panel.PanelName} already exists");
                        panel.Hide(string.Empty);
                    }
                }
            }

            ChangePanel(defaultPanel);
        }

        public static void ChangePanel(string newPanel)
        {
            if (Instance.panels.ContainsKey(newPanel))
            {
                foreach (var panelPair in Instance.panels)
                {
                    if (panelPair.Key == newPanel)
                        panelPair.Value.Show(Instance.currentPanel);
                    else
                        panelPair.Value.Hide(newPanel);
                }
            }

        }
    }
}