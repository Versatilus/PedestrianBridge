namespace PedestrianBridge.Shapes {
    using System.Collections.Generic;
    using System;
    using UnityEngine;
    using Util;
    using Shapes;
    using System.Linq;
    using static Util.NetUtil;
    using static Util.RoundaboutUtil;
    using VectorUtils = Util.VectorUtil;

    public class JunctionWrapper {
        public ushort NodeID { get; private set; } = 0;
        public bool Valid { get; private set; } = false;

        private int _count;
        private List<LWrapper> _corners;
        private List<ushort> _segList;

        public JunctionWrapper(ushort nodeID) {
            NodeID = nodeID;
            _segList = GetCCSegList(nodeID).ToList();
            _count = _segList.Count;
            _corners = new List<LWrapper>(_count);
            if (_count < 3)
                throw new NotImplementedException("number of segments is less than 3");

            for (int i = 0; i < _count; ++i) {
                ushort segID1 = _segList[i], segID2 = _segList[(i + 1) % _count];
                var corner = new LWrapper(segID1, segID2, PrefabUtil.SelectedPrefab);
                //Log.Info($"created L from segments: {segID1} {segID2}");
                if (corner.Valid) {
                    _corners.Add(corner);
                    this.Valid = true;
                }
            }
        }

        public void Create() {
            foreach(var corner in _corners)
                corner.Create();

            for (int i = 0; i < _count; ++i) {
                var startNode = _corners[i].nodeL;
                var endNode = _corners[(i + 1) % _count].nodeL;
                if (startNode != null && endNode != null) {
                    SegmentWrapper segment = new SegmentWrapper(
                        startNode, endNode);
                    segment.Create();
                    TMPEUtil.BanPedestrianCrossings(_segList[(i + 1) % _count], NodeID);
                }
            } // end for
        }
    }
}