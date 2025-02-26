using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Demir.MissionSystem
{
    public class UIRewardContainer : MonoBehaviour
    {
        [Header(" Elements ")]
        [SerializeField] private Image rewardImage;
        [SerializeField] private TextMeshProUGUI rewardLabel;

        public void Configure(Sprite icon, string label)
        {
            rewardImage.sprite = icon;
            rewardLabel.text = label;
        }


    }
}

