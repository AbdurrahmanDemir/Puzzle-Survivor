using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Tabsil.ModernUISystem
{
    [RequireComponent(typeof(RectTransform))]
    public class UIFloatingNote : MonoBehaviour
    {
        [Header(" Elements ")]
        [SerializeField] private RectTransform container;
        [SerializeField] private TextMeshProUGUI primaryText;
        [SerializeField] private TextMeshProUGUI secondaryText;
        private RectTransform rectTransform;

        [Header(" Settings ")]
        float containerWidth;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        private void OnEnable()
        {
            containerWidth = (2f * Screen.width / 3) / container.lossyScale.x;
            container.sizeDelta = container.sizeDelta.With(x: containerWidth);
        }

        public void Configure(Vector2 worldSpacePosition, string primaryString)
        {
            Configure(worldSpacePosition, primaryString, "");
        }

        public void Configure(Vector2 worldSpacePosition, string primaryString, string secondaryString)
        {
            rectTransform.position = worldSpacePosition + 50 * Vector2.down;
            primaryText.text = primaryString;
            secondaryText.text = secondaryString;

            if (Screen.width - worldSpacePosition.x < containerWidth / 2)
            {
                // Move the container to the left
                container.anchoredPosition = container.anchoredPosition.With(x: -containerWidth / 3);
            }
            else if (worldSpacePosition.x < containerWidth / 2)
            {
                // Move the container to the right
                container.anchoredPosition = container.anchoredPosition.With(x: containerWidth / 3);
            }
            else
            {
                // Keep it at the center
                container.anchoredPosition = container.anchoredPosition.With(x: 0);
            }

            transform.localScale = Vector3.zero;
            LeanTween.scale(gameObject, Vector3.one, .2f).setEase(LeanTweenType.easeOutBack);
        }
    }

    public static class VectorExtensions
    {
        public static Vector2 With(this Vector2 vector, float? x = null, float? y = null)
        {
            return new Vector2(x ?? vector.x, y ?? vector.y);
        }

        public static Vector3 With(this Vector3 vector, float? x = null, float? y = null, float? z = null)
        {
            return new Vector3(x ?? vector.x, y ?? vector.y, z ?? vector.z);
        }
    }
}
