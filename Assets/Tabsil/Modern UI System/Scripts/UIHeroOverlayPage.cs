using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tabsil.ModernUISystem
{
    public class UIHeroOverlayPage : UIOverlayPage
    {
        [Header(" Elements ")]
        [SerializeField] private Button upgradeButton;

        [Header(" Popups ")]
        [SerializeField] private UIPopupBase upgradePopup;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            upgradeButton.onClick.RemoveAllListeners();
            upgradeButton.onClick.AddListener(() => PopupManager.Show(upgradePopup));
        }
    }
}