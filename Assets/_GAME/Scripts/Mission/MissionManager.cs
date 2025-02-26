using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Tabsil.Sijil;

namespace Demir.MissionSystem
{
    [RequireComponent(typeof(UIMissionManager))]
    [RequireComponent(typeof(MissionTimer))]
    public class MissionManager : MonoBehaviour
    {

        public static MissionManager instance;

        [Header("Components")]
        private UIMissionManager ui;
        private MissionTimer timer;

        [Header("Data")]
        [SerializeField] private MissionData[] missionDatas;
        List<Mission> activeMissions= new List<Mission> ();

        private int xp;
        public int Xp => xp;

        [Header("Action")]
        public static Action reset;
        public static Action<int> xpUpdated;

        [Header("Particle")]
        [SerializeField] private ObjectFlow flowParticle;

        [Header(" Save / Load")]
        private bool shouldSave;
        private int[] amounts;
        private bool[] claimedStates;
        private const string amountsKey = "MissionDataAmount";
        private const string claimedStatesKey = "MissionClaimedStates";
        private const string xpKey = "MissionXp";

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
                Destroy(gameObject);


            ui = GetComponent<UIMissionManager>();
            timer = GetComponent<MissionTimer>();

            Mission.updated += OnMissionUpdated;
            Mission.completed += OnMissionCompleted;

            StartCoroutine("SaveCoroutine");

        }
        private void OnDestroy()
        {
            Mission.updated -= OnMissionUpdated;
            Mission.completed -= OnMissionCompleted;

        }
       
        private void Start()
        {
            Load();

            for (int i = 0; i < missionDatas.Length; i++)
            {
                activeMissions.Add(new Mission(missionDatas[i], amounts[i], claimedStates[i]));
            }

            ui.Init(activeMissions.ToArray());

            timer.Init(this);
        }

        private void Update()
        {
            //if (Input.GetMouseButtonDown(0))
            //    Increment(EMissionType.PlayRunnerMode, 1);
            if (Input.GetKeyDown(KeyCode.UpArrow))
                Increment(EMissionType.PlayBoatMode, 1);
            if (Input.GetKeyDown(KeyCode.DownArrow))
                Increment(EMissionType.PlayBasketballMode, 1);
        }

        public static void Increment(EMissionType missionType, int addend)
        {
            bool incremented = false;

            for (int i = 0; i < instance.activeMissions.Count; i++)
            {
                if (instance.activeMissions[i].IsComplete || instance.activeMissions[i].IsClaimed)
                    continue;

                if (instance.activeMissions[i].Type == missionType)
                    instance.activeMissions[i].Amount += addend;

                incremented = true;
            }

            if(incremented)
                instance.Save();
        }

        public void OnMissionUpdated(Mission mission)
        {
            ui.UpdateMission(activeMissions.IndexOf(mission));
        }
        public void OnMissionCompleted(Mission mission)
        {

        }

        public void ResetMissions()
        {
            amounts = new int[missionDatas.Length];
            claimedStates = new bool[missionDatas.Length];
            xp = 0;

            Sijil.Remove(this, amountsKey);
            Sijil.Remove(this, claimedStatesKey);
            Sijil.Remove(this, xpKey);

            activeMissions.Clear();

            for (int i = 0; i < missionDatas.Length; i++)
                activeMissions.Add(new Mission(missionDatas[i]));

            ui.Init(activeMissions.ToArray());

            timer.ResetSelf();

            reset?.Invoke();
        }

        public void HandleMissionClaimed(int index)
        {
            shouldSave = true;

            Mission mission = activeMissions[index];
            mission.Claim();

            xp += mission.Data.RewardXp;
            xpUpdated?.Invoke(xp);

            flowParticle.Flow();

        }

        void Load()
        {
            amounts = new int[missionDatas.Length];
            claimedStates = new bool[missionDatas.Length];

            if (Sijil.TryLoad(this, amountsKey, out object _amount))
                amounts = (int[])_amount;
            if (Sijil.TryLoad(this, claimedStatesKey, out object _claimedStates))
                claimedStates = (bool[])_claimedStates;
            if (Sijil.TryLoad(this, xpKey, out object _xp))
                xp = (int)_xp;

        }

        void Save()
        {
            Debug.Log("Saving...");

            for (int i = 0; i < activeMissions.Count; i++)
            {
                amounts[i] = activeMissions[i].Amount;
                claimedStates[i] = activeMissions[i].IsClaimed;
            }

            Sijil.Save(this, amountsKey, amounts);
            Sijil.Save(this, claimedStatesKey, claimedStates);
            Sijil.Save(this, xpKey, xp);
        }
        IEnumerator SaveCoroutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(5);

                if (shouldSave)
                {
                    Save();
                    shouldSave = false;
                }
            }
        }
    }
}

