using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demir.MissionSystem
{

    [CreateAssetMenu(fileName = "MissionData", menuName = "SO/RewardGroupData")]
    public class RewardGroupData: ScriptableObject
    {
        [Header("Data")]
        [SerializeField] private RewardMilestoneData[] rewardMilestoneDatas;
        public RewardMilestoneData[] RewardMilestoneDatas=> rewardMilestoneDatas;
        
    }

    [System.Serializable]
    public struct RewardMilestoneData
    {
        public Sprite icon;
        public RewardEntryData[] rewards;
        public int requiredXp;
        
    }
    [System.Serializable]
    public struct RewardEntryData
    {
        public Sprite rewardIcon;
        public ERewardType type;
        public int amount;
    }
}

