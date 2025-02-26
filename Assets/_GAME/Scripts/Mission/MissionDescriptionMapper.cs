using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demir.MissionSystem
{
    public static class MissionDescriptionMapper
    {
        static MissionDescriptionMap data;

        const string dataPath = "Mission Description Map";

        static MissionDescriptionMapper()
            => data = Resources.Load(dataPath) as MissionDescriptionMap;

        public static string GetDescription(MissionData missionData)
            => data.GetDescription(missionData.Type, missionData.Target);
    }

}
