using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demir.MissionSystem
{
    public class UIMissionManager : MonoBehaviour
    {

        [Header("Elements")]
        [SerializeField] private UIMissionContainer missionContainerPrefab;
        [SerializeField] private Transform missionContainersParent;

        List<UIMissionContainer> activeMissionContainers = new List<UIMissionContainer>();
            
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Init(Mission[] activeMission)
        {
            missionContainersParent.Clear();
            activeMissionContainers.Clear();
            for (int i = 0; i < activeMission.Length; i++)
            {
                UIMissionContainer containerInstance = Instantiate(missionContainerPrefab, missionContainersParent);

                int _i = i;
                
                containerInstance.Configure(activeMission[i], ()=> ClaimMission(_i));

                activeMissionContainers.Add(containerInstance);
            }
            Reorder();
        }
        void Reorder()
        {
            for (int i = 0; i < activeMissionContainers.Count; i++)
            {
                if (activeMissionContainers[i].IsClaimed)
                {
                    activeMissionContainers[i].transform.SetAsLastSibling();
                }
            }
        }
        public void ClaimMission(int index)
        {
            Debug.Log("Claiming missin"+ index);
            activeMissionContainers[index].Claim();
            activeMissionContainers[index].transform.SetAsLastSibling();

            MissionManager.instance.HandleMissionClaimed(index);
        }
        public void UpdateMission(int index)
        {
            activeMissionContainers[index].UpdateVisuals();
        }
    }
}
