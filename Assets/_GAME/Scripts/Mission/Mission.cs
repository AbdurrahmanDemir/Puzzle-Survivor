using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demir.MissionSystem
{
    [Serializable]
    public class Mission
    {

        [Header("Action")]
        public static Action<Mission> updated;
        public static Action<Mission> completed;

        private MissionData data;
        public MissionData Data => data;


        private int amount;
        public int Amount
        {
            get => amount;
            set
            {
                amount = Math.Min(value, data.Target);

                updated?.Invoke(this);

                if (amount == data.Target)
                    Complete();
            }
        }

        public float Progress => (float)amount/ data.Target;
        public string ProgressString => amount + " / " + data.Target;

        private bool isComplete;
        public bool IsComplete => isComplete;

        private bool isClaimed;
        public bool IsClaimed => isClaimed;
        public EMissionType Type => data.Type;

        
        public Mission(MissionData data)
        {
            this.data = data;
        }
        public Mission(MissionData data, int amount, bool claimedState)
        {
            this.data = data;
            this.amount = amount;

            if (claimedState)
                Claim();
        }

        public void Complete()
        {
            Debug.Log("Mission Complete");
            isComplete = true;
            completed?.Invoke(this);
        }
        public void Claim()
        {
            isComplete = true;
            isClaimed = true;
        }
    }
}
