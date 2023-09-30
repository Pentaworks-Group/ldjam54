using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Prefabs.Menues.Book
{
    public class BookBehaviour : MonoBehaviour
    {
        public bool ShouldGenerateIndexPage = false;

        private Int32 currentPageIndex = -1;
        private GameObject pageBackButton;
        private GameObject indexPageButton;
        private GameObject pageForwardButton;


        private List<PageBehaviour> pages;
        private PageBehaviour currentPage;


        public void Awake()
        {
            this.pageBackButton = this.transform.Find("PageButtons/PageBackContainer/PageBackButton").gameObject;
            this.indexPageButton = this.transform.Find("PageButtons/IndexPageButton").gameObject;
            this.pageForwardButton = this.transform.Find("PageButtons/PageForwardContainer/PageForwardButton").gameObject;
            LoadPages();
            if (ShouldGenerateIndexPage)
            {
                GenerateIndexPage();
            }
            OpenPage(0);
        }

        private void LoadPages()
        {
            this.pages = new List<PageBehaviour>();
            foreach (Transform child in this.transform.Find("PageArea"))
            {
                var pageBehaviour = child.gameObject.GetComponent<PageBehaviour>();
                pageBehaviour.SetPageBehaviour(this);
                pages.Add(pageBehaviour);
            }
        }

        public void OpenPage(PageBehaviour page)
        {
            var index = this.pages.IndexOf(page);

            OpenPage(index);
        }

        private void GenerateIndexPage()
        {
            var template = this.transform.Find("IndexPageTemplate").gameObject;
            var pageArea = this.transform.Find("PageArea");
            var indexPage = Instantiate(template, pageArea);

            var numPages = Math.Min(5, pages.Count);
            var buttonTemplate = indexPage.transform.Find("LeftArea/LeftAreaContent/Button").GetComponent<Button>();

            var relativeSize = 0.2f;
            var texts = new List<Text>();
            CreateLinks(numPages, buttonTemplate, relativeSize, 0, texts);
            var remaining = pages.Count - numPages;
            if (remaining > 0)
            {
                buttonTemplate = indexPage.transform.Find("RightArea/RightContentArea/Button").GetComponent<Button>();
                relativeSize = 1f / 6f;
                CreateLinks(remaining, buttonTemplate, relativeSize, numPages, texts);
            }
            StartCoroutine(SetUniformTextSize(texts));
            //OpenPage(indexPage.transform.GetSiblingIndex());
            pages.Insert(0, indexPage.GetComponent<PageBehaviour>());
            indexPage.transform.SetSiblingIndex(0);
            //currentPageIndex = 0;
        }

        private IEnumerator SetUniformTextSize(List<Text> texts)
        {
            yield return new WaitForEndOfFrame();
            var minTextSize = Int32.MaxValue;
            foreach (var text in texts)
            {
                if (text.cachedTextGenerator.fontSizeUsedForBestFit < minTextSize)
                {
                    minTextSize = text.cachedTextGenerator.fontSizeUsedForBestFit;
                }
            }
            foreach (var text in texts)
            {
                text.resizeTextForBestFit = false;
                text.fontSize = minTextSize;
            }
        }

        private void CreateLinks(Int32 numPages, Button buttonTemplate, Single relativeSize, Int32 pageOffset, List<Text> texts)
        {
            for (int i = 0; i < numPages; i++)
            {
                var page = pages[pageOffset + i];
                var pageLink = Instantiate(buttonTemplate, buttonTemplate.transform.parent);
                var text = pageLink.transform.Find("Text").GetComponent<Text>();
                texts.Add(text);
                text.text = page.indexName;
                var rect = pageLink.GetComponent<RectTransform>();
                float top = 1 - (float)i * relativeSize;
                rect.anchorMin = new Vector2(0, top - relativeSize);
                rect.anchorMax = new Vector2(1, top);
                pageLink.onClick.AddListener(pages[i].OpenThisPage);
                pageLink.gameObject.SetActive(true);
            }
        }

        public void OpenPage(Int32 pageIndex)
        {
            if (pageIndex >= 0 && pageIndex < this.pages.Count)
            {
                if (currentPage != null)
                {
                    currentPage.gameObject.SetActive(false);
                }

                currentPage = this.pages[pageIndex];
                currentPageIndex = pageIndex;

                currentPage.gameObject.SetActive(true);

                var canOpenPreviousPage = this.currentPageIndex > 0;
                var canOpenNextPage = this.currentPageIndex < (this.pages.Count - 1);

                this.pageBackButton.SetActive(canOpenPreviousPage);
                this.indexPageButton.SetActive(canOpenPreviousPage);
                this.pageForwardButton.SetActive(canOpenNextPage);

                GameFrame.Base.Audio.Effects.Play("PageFlip");
            }
        }

        public void OnNextPageClicked()
        {
            OpenPage(++this.currentPageIndex);
        }

        public void OnPreviousPageClicked()
        {
            OpenPage(--this.currentPageIndex);
        }
    }

}
