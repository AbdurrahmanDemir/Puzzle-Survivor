using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Tabsil.ModernUISystem
{
    public class UIManager : MonoBehaviour
    {
        [Header(" Elements ")]
        [SerializeField] private RectTransform scrollableRect;
        public RectTransform ScrollableRect => scrollableRect;
        [SerializeField] private RectTransform pagesParent;

        [Header(" Settings ")]
        [SerializeField] private float pageSwapDuration;
        [SerializeField] private LeanTweenType pageSwapEasing;
        private float pageWidth;
        private int currentPageIndex = -1;

        private bool isControllable;
        public bool IsControllable => isControllable; 

        [Header(" Actions ")]
        public Action<int> pageChanged;
        public static Action staticPageChanged;

        private void Awake()
        {
            Application.targetFrameRate = 60;

            PopupManager.popupOpened        += OnPopupOpened;
            PopupManager.allPopupsClosed    += OnAllPopupsClosed;
        }

        private void OnDestroy()
        {
            PopupManager.popupOpened        -= OnPopupOpened;
            PopupManager.allPopupsClosed    -= OnAllPopupsClosed;
        }

        IEnumerator Start()
        {
            isControllable = true;

            yield return null;

            pageWidth = pagesParent.rect.width;

            for (int i = 0; i < pagesParent.childCount; i++)
            {
                RectTransform page = pagesParent.GetChild(i) as RectTransform;
                page.anchoredPosition = Vector2.right * i * pageWidth;
            }

            ShowPage(2);
        }

        public void ShowPage(int pageIndex)
        {
            // Clicking on the same page
            if (pageIndex == currentPageIndex)
                return;

            // Calculate the target parent anchored position
            Vector2 targetParentAnchoredPosition = Vector2.left * pageIndex * pageWidth;

            LeanTween.cancel(scrollableRect);
            LeanTween.move(scrollableRect, targetParentAnchoredPosition, pageSwapDuration).setEase(pageSwapEasing);

            if (pagesParent.GetChild(pageIndex).TryGetComponent(out UIPage uiPage))
                uiPage.Show();

            pageChanged?.Invoke(pageIndex);
            staticPageChanged?.Invoke();

            currentPageIndex = pageIndex;
        }

        public void ShowNextPage()
        {
            // 4 is hard coded because we have 5 pages
            int targetPageIndex = Mathf.Min(currentPageIndex + 1, 4);
            
            // Allows going back to last page if we went further
            currentPageIndex = -1;

            ShowPage(targetPageIndex);
        }

        public void ShowPreviousPage()
        {
            int targetPageIndex = Mathf.Max(0, currentPageIndex - 1);

            // Allows going back to last page if we went further
            currentPageIndex = -1;

            ShowPage(targetPageIndex);
        }

        public void BackToCurrentPage()
        {
            int currentPage = currentPageIndex;
            currentPageIndex = -1;

            ShowPage(currentPage);
        }

        private void OnPopupOpened()
        {
            isControllable = false;
        }

        private void OnAllPopupsClosed()
        {
            isControllable = true;
        }
    }
}