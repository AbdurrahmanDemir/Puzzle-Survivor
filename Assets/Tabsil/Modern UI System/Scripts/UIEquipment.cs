using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tabsil.ModernUISystem
{
    public class UIEquipment : UIPage
    {
        [Header(" Elements ")]
        [SerializeField] private ScrollRect scrollView;
        [SerializeField] private Button heroButton;

        [Header(" Overlay Pages ")]
        [SerializeField] private UIOverlayPage heroPage;

        public override void Show()
        {
            // Reset the Scroll View
            scrollView.verticalNormalizedPosition = 1;
        }

        private void Start()
        {
            heroButton.onClick.RemoveAllListeners();
            heroButton.onClick.AddListener(() => PopupManager.Show(heroPage));
        }
    }
}
