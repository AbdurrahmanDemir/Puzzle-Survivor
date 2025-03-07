using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Tabsil.Sijil;
using System;

namespace Demir.MissionSystem
{
    public class MainMissionSlider : MonoBehaviour
    {
        [Header("Elements")]
        [SerializeField] private Slider slider;
        [SerializeField] private UISliderItem itemPrefabs;
        [SerializeField] private UISliderItem keyItemPrefabs;
        [SerializeField] private RectTransform itemParent;
        private TextMeshProUGUI xpText;
        public TextMeshProUGUI XpText => xpText;

        [Header("Other Elements")]
        [SerializeField] private GameObject rewardPopUp;
        [SerializeField] private MenuManager menuManager;
        [SerializeField] private UIRewardContainer rewardContainerPrefab;
        [SerializeField] private Transform rewardContainersParent;

        [Header("Settings")]
        [SerializeField] private Sprite currencyIcon;
        private List<UISliderItem> sliderItem = new List<UISliderItem>();

        [Header("Actions")]
        public static Action<RewardEntryData[]> rewarded;

        [Header("Data")]
        [SerializeField] private RewardGroupData data;
        private int lastRewardIndex;
        private bool[] itemsOpened;
        private const string lastRewardIndexKey = "MissionLastRewardIndex";
        private const string itemsOpenedKey = "MissionItemsOpend";

        private void Awake()
        {
            MissionManager.xpUpdated += OnXpUpdated;
            MissionManager.reset += ResetSelf;
        }
        private void OnDestroy()
        {
            MissionManager.xpUpdated -= OnXpUpdated;
            MissionManager.reset -= ResetSelf;
        }
        IEnumerator Start()
        {
            yield return null;

            lastRewardIndex = 0;
            itemsOpened = new bool[data.RewardMilestoneDatas.Length];

            Load();
            Init();

        }
        void Init()
        {
            InitSlider();
            GenerateSliderItems();
            UpdateVisuals(MissionManager.instance.Xp);
            AnimateUnopenedItems();
        }

        void GenerateSliderItems()
        {
            itemParent.Clear();
            sliderItem.Clear();

            UISliderItem keyItem = Instantiate(keyItemPrefabs, itemParent);
            keyItem.Configure(currencyIcon,0.ToString());

            xpText = keyItem.Text;


            for (int i = 0; i < data.RewardMilestoneDatas.Length; i++)
            {
                RewardMilestoneData milestoneData = data.RewardMilestoneDatas[i];

                UISliderItem itemInstance = Instantiate(itemPrefabs, itemParent);
                itemInstance.Configure(milestoneData.icon, milestoneData.requiredXp.ToString());

                int _i = i;
                itemInstance.Button.onClick.AddListener(() => HandleSliderItemPressed(_i));

                sliderItem.Add(itemInstance);
            }

            PlaceItems();
        }
        void HandleSliderItemPressed(int index)
        {
            bool canOpen = lastRewardIndex > index;
            bool isOpened = itemsOpened[index];

            if (!canOpen||isOpened)
                return;

            OpenReward(index);
        }

        void OpenReward(int index)
        {
            itemsOpened[index] = true;

            rewardContainersParent.Clear();

            //itemParent.GetChild(index + 1).GetComponent<UISliderItem>().StopAnimation();
            menuManager.TogglePanel(rewardPopUp);
            sliderItem[index].StopAnimation();

            RewardEntryData[] entryData = data.RewardMilestoneDatas[index].rewards;

            for (int i = 0; i < entryData.Length; i++)
            {
                RewardEntryData data = entryData[i];
                UIRewardContainer containerInstance = Instantiate(rewardContainerPrefab, rewardContainersParent);
                containerInstance.Configure(data.rewardIcon, data.amount.ToString());

                switch (data.type)
                {
                    case ERewardType.Coin:
                        DataManager.instance.AddGold(data.amount);
                        break;
                    case ERewardType.Energy:
                        DataManager.instance.AddGold(data.amount);
                        break;
                    case ERewardType.Eqipment_01:
                        break;
                    case ERewardType.Eqipment_02:
                        break;
                    case ERewardType.Special_01:
                        break;
                    case ERewardType.Special_02:
                        break;
                    default:
                        break;
                }
            }


            Save();

            rewarded?.Invoke(data.RewardMilestoneDatas[index].rewards);
           
        }
        private void PlaceItems()
        {
            float width = itemParent.rect.width;
            float spacing = width / (itemParent.childCount - 1);

            Vector2 startPosition = (Vector2)itemParent.position - Vector2.right * width / 2;

            for (int i = 0; i < itemParent.childCount; i++)
                itemParent.GetChild(i).position = startPosition + spacing * i * Vector2.right;
        }


        void InitSlider()
        {
            slider.minValue = 0;
            slider.maxValue = data.RewardMilestoneDatas[data.RewardMilestoneDatas.Length - 1].requiredXp;

            slider.value = 0;
        }

        void CheckForRewards()
        {
            if (lastRewardIndex > data.RewardMilestoneDatas[lastRewardIndex].requiredXp)
                return;

            if (slider.value >= data.RewardMilestoneDatas[lastRewardIndex].requiredXp)
                EnableReward();
        }
        void EnableReward()
        {
            UISliderItem item= itemParent.GetChild(lastRewardIndex+1).GetComponent<UISliderItem>();
            item.Animate();

            sliderItem[lastRewardIndex].Animate();

            lastRewardIndex++;
        }
        void OnXpUpdated(int xp)
        {
            UpdateVisuals(xp);
            CheckForRewards();
        }
        void UpdateVisuals(int xp)
        {
            slider.value = xp;
            xpText.text = xp.ToString();
        }
        private void AnimateUnopenedItems()
        {
            for (int i = 0; i < sliderItem.Count; i++)
                if (!itemsOpened[i] && slider.value >= data.RewardMilestoneDatas[i].requiredXp)
                    sliderItem[i].Animate();
        }
        private void Load()
        {
            if (Sijil.TryLoad(this, lastRewardIndexKey, out object _lastRewardIndex))
                lastRewardIndex = (int)_lastRewardIndex;

            if (Sijil.TryLoad(this, itemsOpenedKey, out object _itemsOpened))
                itemsOpened = (bool[])_itemsOpened;
        }

        private void Save()
        {
            Sijil.Save(this, lastRewardIndexKey, lastRewardIndex);
            Sijil.Save(this, itemsOpenedKey, itemsOpened);
        }

        private void ResetSelf()
        {
            Sijil.Remove(this, lastRewardIndexKey);
            Sijil.Remove(this, itemsOpenedKey);

            StartCoroutine("Start");
        }
    }
}

