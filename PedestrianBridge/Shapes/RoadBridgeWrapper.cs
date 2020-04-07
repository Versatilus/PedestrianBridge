namespace PedestrianBridge.Shapes {
    using System.Collections.Generic;
    using System;
    using UnityEngine;
    using Util;
    using Shapes;
    using System.Linq;
    using static Util.NetUtil;
    using static Util.RoundaboutUtil;
    using ColossalFramework;

    public class RoadBridgeWrapper {
        public const int MIN_SEGMENT_COUNT = 2;
        public ushort NodeID { get; private set; } = 0;
        public bool IsValid => _side1.IsValid; //&& _side2.IsValid;

        RoadSideWrapper _side1;
        RoadSideWrapper _side2;
        SegmentWrapper _bridge;
        public RoadBridgeWrapper(ushort segmentID, float t,  NetInfo pathInfo) {
            _side1 = new RoadSideWrapper(segmentID, t, pathInfo, leftSide: false);
            //_side2 = new RoadSideWrapper(segmentID, t, pathInfo, leftSide: true);
            //if(IsValid)_bridge = new SegmentWrapper(_side1.node0, _side2.node0);
        }

        public static void Create(ushort segmentID, Vector3 HitPos) {
            float t = segmentID.ToSegment().GetClosestT(HitPos);
            Create(segmentID, t);
        }

        public static void Create(ushort segmentID, float t) {
            var roadBridge = new RoadBridgeWrapper(segmentID, t, PrefabUtil.SelectedPrefab);
            if (roadBridge.IsValid)
                roadBridge.Create();
        }

        public static bool RenderOverlay(
            RenderManager.CameraInfo cameraInfo, Color color, ushort segmentID, Vector3 hitPos) {
            Vector3 pos = segmentID.ToSegment().GetClosestPosition(hitPos);
            float hw = segmentID.ToSegment().Info.m_halfWidth;

            Singleton<ToolManager>.instance.m_drawCallData.m_overlayCalls++;
            RenderManager.instance.OverlayEffect.DrawCircle(
                cameraInfo,
                color,
                pos,
                hw * 2,
                -1f,
                1280f,
                renderLimits: false,
                alphaBlend: true);

            Singleton<ToolManager>.instance.m_drawCallData.m_overlayCalls++;
            RenderManager.instance.OverlayEffect.DrawCircle(
                cameraInfo,
                Color.red,
                hitPos,
                hw * 1,
                -1f,
                1280f,
                renderLimits: false,
                alphaBlend: true);
            return true;
        }

        public void Create() {
            if (!IsValid)
                return;
            _side1?.Create();
            _side2?.Create();
            _bridge?.Create();
        }


    }
}