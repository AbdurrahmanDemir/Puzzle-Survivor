using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demir.MissionSystem
{
    [CreateAssetMenu(fileName ="MissionData", menuName = "SO/MissionData")]
    public class MissionData : ScriptableObject
    {
        [SerializeField] private EMissionType type;
        public EMissionType Type => type;

        [SerializeField] private int target;
        public int Target => target;

        [SerializeField] private int rewardXp;
        public int RewardXp => rewardXp;

        [SerializeField] private Sprite icon;
        public Sprite Icon => icon;
    }
}

