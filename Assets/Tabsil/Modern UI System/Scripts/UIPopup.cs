using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tabsil.ModernUISystem
{
    public class UIPopup : UIPopupBase
    {
        [Header(" Animation ")]
        [SerializeField] private Image background;
        [SerializeField] private RectTransform mainContainer;
        [SerializeField] private float animationDuration;
        [SerializeField] private LeanTweenType openEase;

        [Header(" Interactables ")]
        [SerializeField] private Button closeButton;

        bool isClosing;

        // Start is called before the first frame update
        void Start()
        {
            closeButton.onClick.RemoveAllListeners();
            closeButton.onClick.AddListener(Close);

            Open();
        }

        private void Open()
        {
            float backgroundTargetAlpha = background.color.a;
            background.color = Color.clear;

            LeanTween.alpha(background.rectTransform, backgroundTargetAlpha, animationDuration).setRecursive(false);

            mainContainer.localScale = Vector3.zero;
            LeanTween.scale(mainContainer, Vector3.one, animationDuration).setEase(openEase);
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