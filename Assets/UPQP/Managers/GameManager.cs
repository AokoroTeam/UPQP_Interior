using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Aokoro;

namespace UPQP.Managers
{
    [DefaultExecutionOrder(-100)]
    [AddComponentMenu("UPQP/Managers/GameManager")]
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] GameObject LoosedFocusUI;

        public InfluencedProperty<CursorLockMode> cursorLockMode;
        public InfluencedProperty<bool> cursorVisibility;

        private void OnEnable()
        {
            cursorLockMode.OnValueChanged += CursorLockMode_OnValueChanged;
            cursorVisibility.OnValueChanged += CursorVisibility_OnValueChanged;
        }

        private void OnDisable()
        {
            cursorLockMode.OnValueChanged -= CursorLockMode_OnValueChanged;
            cursorVisibility.OnValueChanged -= CursorVisibility_OnValueChanged;
        }

        private void Start()
        {
            OnFocusLost();
        }


        private void CursorVisibility_OnValueChanged(bool value, object key)
        {
            Cursor.visible = value;
        }
        private void CursorLockMode_OnValueChanged(CursorLockMode value, object key) => Cursor.lockState = value;

        public void OnFocusLost()
        {
            cursorLockMode.Subscribe(this, PriorityTags.Highest, CursorLockMode.Confined);
            cursorVisibility.Subscribe(this, PriorityTags.Highest, true);

            LoosedFocusUI.SetActive(true);

            Time.timeScale = 0;
        }


        public void OnFocusRegained()
        {
            cursorLockMode.Unsubscribe(this);
            cursorVisibility.Unsubscribe(this);

            LoosedFocusUI.SetActive(false);
            Screen.fullScreen = true;
            Time.timeScale = 1;
        }

        public void OnFocusRegainedAnimation()
        {
            Invoke(nameof(OnFocusRegained), .5f);
        }

        protected override void OnExistingInstanceFound(GameManager existingInstance)
        {
            Destroy(gameObject);
        }
    }
}
