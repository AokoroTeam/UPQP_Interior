using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem ;
using Aokoro;

namespace UPQP.Managers
{
    [DefaultExecutionOrder(-100)]
    [AddComponentMenu("UPQP/Managers/GameManager")]
    public class GameManager : Singleton<GameManager>
    {
        public bool IsGameFocused { get; private set; }

        [SerializeField] GameObject LoosedFocusUI;
        [SerializeField] InputAction echapAction;
        
        protected override void Awake()
        {
            base.Awake();
        }


        
        private void Start()
        {
            IsGameFocused = Application.isFocused;
            OnFocusLost();
        }

        
        private void OnEnable()
        {
            echapAction.performed += EchapAction_performed;
            Application.focusChanged += OnFocusChanged;
        }

        private void OnDisable()
        {
            echapAction.performed -= EchapAction_performed;
            Application.focusChanged -= OnFocusChanged;
        }


        public void OnFocusLost()
        {
            echapAction.Disable();
            LoosedFocusUI.SetActive(true);
            Time.timeScale = 0;
        }


        public void OnFocusRegained()
        {
            echapAction.Enable();
            LoosedFocusUI.SetActive(false);

            Time.timeScale = 1;
        }


        private void OnFocusChanged(bool focus)
        {
            if (!Application.isEditor)
            {
                if(focus)
                    OnFocusRegained();
                else
                    OnFocusLost();

                LoosedFocusUI.SetActive(!focus);
            }

            Debug.Log(focus);
            IsGameFocused = focus;
        }

        private void EchapAction_performed(InputAction.CallbackContext ctx) => OnFocusChanged(true);
        
        
        protected override void OnExistingInstanceFound(GameManager existingInstance)
        {
            Destroy(gameObject);
        }
    }
}
