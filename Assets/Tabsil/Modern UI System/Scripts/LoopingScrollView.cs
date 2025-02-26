using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

namespace Tabsil.ModernUISystem
{
    public class LoopingScrollView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [Header(" Elements ")]
        [SerializeField] private RectTransform elementsParent;
        private Transform[] elements;

        [Header(" Settings ")]
        private float dragStartPositionX;
        private float elementWidth;
        private Vector2 parentInitialPosition;
        private bool dragging;
        private bool hasSwitched;
        private int currentIndex;

        [Header(" Lerp ")]
        [SerializeField] private AnimationCurve scaleCurve;
        [SerializeField] private float scaleLerp;

        [Header(" Actions ")]
        public Action<int> elementChanged;

        private void Awake()
        {
            elementWidth = elementsParent.GetChild(0).GetComponent<RectTransform>().rect.width;

            elements = new Transform[elementsParent.childCount];
            for (int i = 0; i < elementsParent.childCount; i++)
                elements[i] = elementsParent.GetChild(i);            
        }
      
        private void Update()
        {
            if (dragging)
            {
                MoveElementsParent();

                if(!hasSwitched)
                    UpdateElementsScale();
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            LeanTween.cancel(elementsParent);

            dragStartPositionX = Input.mousePosition.x;
            parentInitialPosition = elementsParent.anchoredPosition;

            dragging = true;
        }

        private void MoveElementsParent()
        {
            hasSwitched = false;

            float xDistance = Input.mousePosition.x - dragStartPositionX;
            Vector2 targetAnchoredPosition = parentInitialPosition + xDistance * Vector2.right;

            if (targetAnchoredPosition.x > elementWidth / 2)
            {
                // Last child will be the first
                elementsParent.GetChild(elementsParent.childCount - 1).SetAsFirstSibling();

                // Move left by elementWidth
                targetAnchoredPosition.x -= elementWidth;

                parentInitialPosition = targetAnchoredPosition;
                dragStartPositionX = Input.mousePosition.x;

                currentIndex--;

                if (currentIndex == -1)
                    currentIndex = elements.Length - 1;

                elementChanged?.Invoke(currentIndex);

                hasSwitched = true;
            }

            else if (targetAnchoredPosition.x < -elementWidth / 2)
            {
                elementsParent.GetChild(0).SetAsLastSibling();

                targetAnchoredPosition.x += elementWidth;
                parentInitialPosition = targetAnchoredPosition;
                dragStartPositionX = Input.mousePosition.x;

                currentIndex++;

                if (currentIndex == elements.Length)
                    currentIndex = 0;

                elementChanged?.Invoke(currentIndex);

                hasSwitched = true;
            }        

            elementsParent.anchoredPosition = targetAnchoredPosition;
        }

        private void UpdateElementsScale(float value = -1f)
        {
            // Manage the children scales
            for (int i = 0; i < elements.Length; i++)
            {
                RectTransform element = elements[i].GetComponent<RectTransform>();

                float pixelX = Mathf.Abs(element.position.x) - (Screen.width / 2);

                pixelX = Mathf.Abs(pixelX);
                pixelX = Mathf.Clamp(pixelX, 0, elementWidth * 2);

                float targetScale = 1 - scaleCurve.Evaluate((pixelX / (elementWidth * 2)));

                float t = Time.deltaTime * scaleLerp;

                if (value > 0)
                    t = value / 2;

                // Scale
                element.localScale = Vector3.Lerp(element.localScale, Vector3.one * targetScale, t);

                // Alpha
                //
                /*
                Color color = new Color(1, 1, 1);
                color.a = Mathf.Lerp(color.a, targetScale, t);
                element.GetChild(0).GetComponent<Image>().color = color;
                */
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            dragging = false;

            // Here I can lerp to the closest element
            LeanTween.move(elementsParent, Vector2.zero, .25f)
                .setEase(LeanTweenType.easeInOutSine)
                .setOnUpdate((value) => UpdateElementsScale(value));
        }

        public void OnDrag(PointerEventData eventData)
        {
            // Nothing interesting here...
            // But this is still useful =)
        }
    }
}