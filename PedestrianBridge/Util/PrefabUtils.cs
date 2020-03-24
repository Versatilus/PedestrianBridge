namespace PedestrianBridge.Util {
    using ColossalFramework;
    using System;
    using static HelpersExtensions;

    public static class  PrefabUtils {
        public static NetInfo SelectedPrefab => defaultPrefab
        public static NetInfo defaultPrefab => PedestrianPathInfo;
        public static NetInfo PedestrianBridgeInfo =>
            GetInfo("Pedestrian Elevated");
        public static NetInfo PedestrianPathInfo =>
            GetInfo("Pedestrian Pavement");

        public static NetInfo GetInfo(string name) {
            int count = PrefabCollection<NetInfo>.LoadedCount();
            for (uint i = 0; i < count; ++i) {
                NetInfo info = PrefabCollection<NetInfo>.GetLoaded(i);
                if (info.name == name)
                    return info;
                //Helpers.Log(info.name);
            }
            throw new Exception("NetInfo not found!");
        }
        public static NetInfo GetElevated(this NetInfo info) {
            NetAI ai = info.m_netAI;
            if (ai is PedestrianBridgeAI || ai is RoadBridgeAI)
                return info;

            if (ai is PedestrianPathAI)
                return (ai as PedestrianPathAI).m_elevatedInfo;

            if (ai is RoadAI)
                return (ai as RoadAI).m_elevatedInfo;
            return null;
        }

    }
}
