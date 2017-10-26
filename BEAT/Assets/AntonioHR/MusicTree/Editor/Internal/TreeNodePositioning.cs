using AntonioHR.TreeAsset;
using AntonioHR.TreeAsset.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AntonioHR.MusicTree.Editor.Internal
{
    public class TreeNodePositioning<TNP, RT, TA, TNA, RTN>
        where TNP : TreeNodePositioning<TNP, RT, TA, TNA, RTN>, new()
        where RT : RuntimeTree<RT, TA, TNA, RTN>, new()
        where TA : TreeAsset<TNA>
        where TNA : TreeNodeAsset
        where RTN : RuntimeTreeNode<RT, TA, TNA, RTN>, new()
    {
        #region Data Structures
        public class TreePositionParameters
        {
            public int sibilingSeparation = 120;
            public int subTreeSeparation = 140;

            public int levelSeparation = 100;

            public int yStart = 100;
            public int xStart = 100;

            public int nodeSize = 50;
        }

        public class NodePositioning
        {
            public RTN node;

            public float localX;
            public float mod;
            public float h;

            public List<float> leftContour = new List<float>();
            public List<float> rightContour = new List<float>();

            public Rect bounds;
        }
        #endregion

        private NodePositioning[] nodePositionings;
        private RT tree;
        private TreePositionParameters positionParams;

        private Rect baseRect;
       

        protected TreeNodePositioning()
        {

        }

        public Rect GetBoundsFor(RTN node)
        {
            return nodePositionings[node.NodeId].bounds;
        }

        public Vector2 GetTreeSize()
        {
            return new Vector2(800, 900);
        }



        public static TNP CreateFrom(RT t)
        {
            return CreateFrom(t, DefaultPosParameters());
        }

        public static TreePositionParameters DefaultPosParameters()
        {
            return new TreePositionParameters();
        }

        public static TNP CreateFrom(RT t, TreePositionParameters posParameters)
        {
            TNP result = new TNP()
                {
                    tree = t,
                    baseRect = new Rect(0, 0, posParameters.nodeSize, posParameters.nodeSize),
                    positionParams = posParameters
                };


            result.RunFirstPass();

            result.RunSecondPass();

            result.CalculateRects();

            return result;
        }

        private void RunFirstPass()
        {
            nodePositionings = new NodePositioning[tree.NodeCount];
            foreach (var n in tree.AllNodes)
            {
                InitializeNodeWithBasePositions(n);
            }
        }

        private void InitializeNodeWithBasePositions(RTN n)
        {
            nodePositionings[n.NodeId] = new NodePositioning()
            {
                node = n,
                localX = CalculateBaseXPosFor(n.SibilingIndex),
                h = CalculateBaseYPosFor(n.Depth)
            };
        }

        private float CalculateBaseXPosFor(int sibilingIndex)
        {
            return positionParams.xStart + sibilingIndex * (positionParams.sibilingSeparation + positionParams.nodeSize);
        }

        private float CalculateBaseYPosFor(int height)
        {
            return positionParams.yStart + height * (positionParams.levelSeparation + positionParams.nodeSize);
        }




        private void RunSecondPass()
        {
            foreach (var node in tree.Root.Postorder())
            {
                PositionNodeOverChildren(node);
            }

            CalculateRects();

            foreach (var node in tree.Root.Postorder())
            {
                HandleOverlapsFor(node);
            }

            CalculateRects();
        }


        void PositionNodeOverChildren(RTN node)
        {
            float desiredValue = getPosDataFor(node).localX;
            var childCount = node.Children.Count;
            if (childCount == 1)
            {
                var lChildPos = getPosDataFor(node.Children[0]);
                desiredValue = lChildPos.localX;
            }
            else if (childCount > 1)
            {
                var lChildPos = getPosDataFor(node.LeftmostChild);
                var rChildPos = getPosDataFor(node.RightmostChild);
                desiredValue = (rChildPos.localX + lChildPos.localX) / 2;
            }

            if (node.LeftSibilingsCount == 0)
            {
                getPosDataFor(node).localX = desiredValue;
            }
            else
            {
                var pos = getPosDataFor(node);
                pos.mod = pos.localX - desiredValue;
            }
        }



        private void HandleOverlapsFor(RTN node)
        {

            var nodePos = getPosDataFor(node);

            UpdateLeftContour(node);

            var shift = 0.0f;
            var left = node.LeftSibiling;
            if(left != null)
            {
                var leftPos = getPosDataFor(left);
                for (int i = node.Depth+1; i <= Mathf.Min(node.SubtreeDepth, left.SubtreeDepth) ; i++)
                {
                    var indx = i - node.Depth;
                    var extraOffset = positionParams.sibilingSeparation + positionParams.nodeSize / 2;

                    var delta = (leftPos.rightContour[indx] + extraOffset) - nodePos.leftContour[indx];

                    shift = Mathf.Max(delta, shift);
                }
            }

            nodePos.localX += shift;
            nodePos.mod += shift;

            UpdateRightContour(node);

        }

        private void UpdateLeftContour(RTN node)
        {
            var nodePos = getPosDataFor(node);
            nodePos.leftContour.Add(nodePos.localX);

            for (int i = node.Depth + 1; i <= node.SubtreeDepth; i++)
            {
                var leftmost = getPosDataFor(node.LeftmostDescendantOfDepth(i));
                nodePos.leftContour.Add(leftmost.leftContour[0] + nodePos.mod);
            }
        }

        private void UpdateRightContour(RTN node)
        {
            var nodePos = getPosDataFor(node);
            nodePos.rightContour.Add(nodePos.localX);

            var left = node.LeftSibiling;
            if (left == null)
            {
                for (int i = node.Depth + 1; i <= node.SubtreeDepth; i++)
                {
                    var rightmost = getPosDataFor(node.RightmostDescendantOfDepth(i));
                    nodePos.rightContour.Add(rightmost.rightContour[0] + nodePos.mod);
                }
            } else
            {
                var leftPos = getPosDataFor(left);
                for (int i = node.Depth + 1; i <= Mathf.Max(node.SubtreeDepth, left.SubtreeDepth); i++)
                {
                    if (node.SubtreeDepth >= i)
                    {
                        var rightmost = getPosDataFor(node.RightmostDescendantOfDepth(i));
                        nodePos.rightContour.Add(rightmost.rightContour[0] + nodePos.mod);
                    } else
                    {
                        leftPos.rightContour.Add(leftPos.rightContour[i - left.Depth]);
                    }
                }
            }
        }

        private void CalculateRects()
        {
            CalculateRectsRecursion(tree.Root);
        }

        private void CalculateRectsRecursion(RTN node, float mod = 0)
        {
            var p = getPosDataFor(node);
            p.bounds = baseRect.At(new Vector2(p.localX + mod, p.h));

            foreach (var child in node.Children)
            {
                CalculateRectsRecursion(child, mod + p.mod);
            }

        }




        private NodePositioning getPosDataFor(RTN node)
        {
            return nodePositionings[node.NodeId];
        }
    }
}
