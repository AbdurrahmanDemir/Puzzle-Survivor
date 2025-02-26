using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tabsil.ModernUISystem
{
    [RequireComponent(typeof(CanvasGroup))]
    public class UIOverlayPage : UIPopupBase
    {
        [Header(" Elements ")]
        private CanvasGroup cg;

        [Header(" Settings ")]
        [SerializeField] private float animationDuration;

        [Header(" Interactables ")]
        [SerializeField] private Button closeButton;

        bool isClosing;

        // Start is called before the first frame update
        protected virtual void Start()
        {
            cg = GetComponent<CanvasGroup>();
            cg.alpha = 0;

            closeButton.onClick.RemoveAllListeners();
            closeButton.onClick.AddListener(Close);

            Open();
        }

        private void Open()
        {
            // In here, let's fade the page in   
            LeanTween.value(gameObject, 0, 1, animationDuration).setOnUpdate((value) => cg.alpha = value);
        }

        public void Close()
        {
            if (isClosing)
                return;

            isClosing = true;

            // Animate then destroy
            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            closed?.Invoke(this);
        }
    }
}