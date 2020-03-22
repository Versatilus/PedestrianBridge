using UnityEngine;
using PedestrianBridge.Util;
using static PedestrianBridge.Util.PrefabUtils;
using static PedestrianBridge.Util.NetUtil;

namespace PedestrianBridge.Shapes {
    public class NodeWrapper {
        Vector2 point;
        byte elevation;
        NetInfo info;
        public ushort ID;

        public NodeWrapper(Vector2 point, byte elevation, NetInfo info) {
            this.point = point;
            this.elevation = elevation;
            if (elevation >= 1)
                info = info.GetElevated();
            this.info = info;
        }

        public void Create() =>
            simMan.AddAction(_Create);

        void _Create() {
            Vector3 pos = Get3DPos(point, elevation);
            ID = CreateNode(pos, info);
            ID.ToNode().m_elevation = elevation;
            if (elevation == 0) {
                ID.ToNode().m_flags &= ~NetNode.Flags.Moveable;
                ID.ToNode().m_flags |= NetNode.Flags.Transition | NetNode.Flags.OnGround;
            }
        }

        static ushort CreateNode(Vector3 position, NetInfo info = null) {
            info = info ?? PedestrianBridgeInfo;
            Log.Info($"creating node for {info.name} at position {position.ToString("000.000")}");
            bool res = netMan.CreateNode(node: out ushort nodeID, randomizer: ref simMan.m_randomizer,
                info: info, position: position, buildIndex: simMan.m_currentBuildIndex);
            if (!res)
                throw new NetServiceException("Node creation failed");
            simMan.m_currentBuildIndex++;
            return nodeID;
        }
        public static Vector3 Get3DPos(Vector2 point, byte elevation) {
            float terrainH = terrainMan.SampleDetailHeightSmooth(point.ToPos(0));
            return point.ToPos(terrainH + elevation);
        }
    }
}