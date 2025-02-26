using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

namespace Tabsil.ModernUISystem
{
    [RequireComponent(typeof(UIManager))]
    public class UIPageScroll : MonoBehaviour
    {
        public enum ScrollType
        {
            None = 0,
            Horizontal = 1,
            Vertical = 2
        }

        [Header(" Elements ")]
        [SerializeField] private RectTransform scrollZone;
        private UIManager uiManager;

        [Header(" Settings ")]
        [SerializeField] private float startScrollThreshold;
        private bool canStartScrolling;

        [Header(" Data ")]
        private Vector2 clickedPosition;
        private Vector2 scrollableRectStartPosition;
        private ScrollType scrollType = ScrollType.None;

        private StandaloneInputModuleCustom inputModule;
        PointerEventData pointerData;

        public Vector2 pointerPosition;
        public bool dragging;

        private void Awake()
        {
            uiManager = GetComponent<UIManager>();
            inputModule = EventSystem.current.GetComponent<StandaloneInputModuleCustom>();
        }

        // Update is called once per frame
        void Update()
        {          
            if(uiManager.IsControllable && !IsControllingHorizontalScroll())
                ManageScroll();
        }

        private bool IsControllingHorizontalScroll()
        {
#if UNITY_EDITOR
            pointerData = inputModule.GetLastPointerEventDataPublic(-1);
#else
            pointerData = inputModule.GetLastPointerEventDataPublic(0);
#endif


            if (pointerData == null)
                return false;            

            if (pointerData.pointerDrag == null)
                return false;

            if (pointerData.pointerDrag.TryGetComponent(out LoopingScrollView lsv))
                return true;

            if (pointerData.pointerDrag.TryGetComponent(out ScrollRect scrollRect))
            {
                if (scrollRect.horizontal)
                    return true;
            }

            return false;
        }

        private void ManageScroll()
        {
            if (Input.GetMouseButtonDown(0))
                ManageMouseDown();

            else if (Input.GetMouseButton(0))
                MouseDrag();

            else if (Input.GetMouseButtonUp(0))
                ManageMouseUp();
        }

        private void ManageMouseDown()
        {
            clickedPosition = Input.mousePosition;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(scrollZone, clickedPosition, null, out Vector2 rectangleLocalPoint);
            canStartScrolling = scrollZone.rect.Contains(rectangleLocalPoint);
        }

        private void MouseDrag()
        {
            if (!canStartScrolling)
                return;


            float xDistance = Input.mousePosition.x - clickedPosition.x;
            float yDistance = Input.mousePosition.y - clickedPosition.y;

            if (scrollType == ScrollType.None)
            {
                TryStartingScroll(xDistance, yDistance);
                return;
            }

            if (scrollType == ScrollType.Horizontal)
                ManageHorizontalScroll(xDistance);
            else
                ManageVerticalScroll(yDistance);
        }

        private void TryStartingScroll(float xDistance, float yDistance)
        {
            // Check if one of those two is above the threshold
            if (Mathf.Abs(xDistance) > startScrollThreshold)
                StartHorizontalScroll();

            else if (Mathf.Abs(yDistance) > startScrollThreshold)
                StartVerticalScroll();
        }

        private void StartHorizontalScroll()
        {
            scrollType = ScrollType.Horizontal;
            scrollableRectStartPosition = uiManager.ScrollableRect.anchoredPosition;
        }

        private void ManageHorizontalScroll(float distance)
        {
            Vector2 targetPosition = scrollableRectStartPosition + Vector2.right * distance;
            uiManager.ScrollableRect.anchoredPosition = targetPosition;
        }

        private void StartVerticalScroll()
        {
            scrollType = ScrollType.Vertical;    
        }

        private void ManageVerticalScroll(float distance)
        {

        }

        private void ManageMouseUp()
        {
            if (scrollType == ScrollType.None)
                return;

            if (scrollType == ScrollType.Horizontal)
                EndHorizontalScroll();

            scrollType = ScrollType.None;
            canStartScrolling = false;
        }

        private void EndHorizontalScroll()
        {
            float distance = Input.mousePosition.x - clickedPosition.x;

            if (Mathf.Abs(distance) > Screen.width / 6)
            {
                // We should try and switch pages
                if (distance > 0)
                    uiManager.ShowPreviousPage();
                else
                    uiManager.ShowNextPage();
            }
            else
                uiManager.BackToCurrentPage();
        }
    }
}