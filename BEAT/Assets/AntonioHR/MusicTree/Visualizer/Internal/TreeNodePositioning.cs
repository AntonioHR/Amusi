using AntonioHR.TreeAsset;
using AntonioHR.TreeAsset.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AntonioHR.MusicTree.Visualizer.Internal
{
    public class TreeNodePositioning<T> where T:TreeNodeAsset
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
            public ReadonlyTreeNode<T> node;

            public float localX;
            public float mod;
            public float h;

            public List<float> leftContour = new List<float>();
            public List<float> rightContour = new List<float>();

            public Rect bounds;
        }
        #endregion

        private NodePositioning[] nodePositionings;
        private ReadonlyTree<T> tree;
        private TreePositionParameters positionParams;

        private Rect baseRect;
       

        private TreeNodePositioning()
        {

        }

        public Rect GetBoundsFor(ReadonlyTreeNode<T> node)
        {
            return nodePositionings[node.NodeId].bounds;
        }

        public Vector2 GetTreeSize()
        {
            return new Vector2(800, 900);
        }



        public static TreeNodePositioning<T> CreateFrom(ReadonlyTree<T> t)
        {
            return CreateFrom(t, DefaultPosParameters());
        }

        public static TreePositionParameters DefaultPosParameters()
        {
            return new TreePositionParameters();
        }

        public static TreeNodePositioning<T> CreateFrom(ReadonlyTree<T> t, TreePositionParameters posParameters)
        {
            TreeNodePositioning<T> result = new TreeNodePositioning<T>()
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

        private void InitializeNodeWithBasePositions(ReadonlyTreeNode<T> n)
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


        void PositionNodeOverChildren(ReadonlyTreeNode<T> node)
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



        private void HandleOverlapsFor(ReadonlyTreeNode<T> node)
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

        private void UpdateLeftContour(ReadonlyTreeNode<T> node)
        {
            var nodePos = getPosDataFor(node);
            nodePos.leftContour.Add(nodePos.localX);

            for (int i = node.Depth + 1; i <= node.SubtreeDepth; i++)
            {
                var leftmost = getPosDataFor(node.LeftmostDescendantOfDepth(i));
                nodePos.leftContour.Add(leftmost.leftContour[0] + nodePos.mod);
            }
        }

        private void UpdateRightContour(ReadonlyTreeNode<T> node)
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

        private void CalculateRectsRecursion(ReadonlyTreeNode<T> node, float mod = 0)
        {
            var p = getPosDataFor(node);
            p.bounds = baseRect.At(new Vector2(p.localX + mod, p.h));

            foreach (var child in node.Children)
            {
                CalculateRectsRecursion(child, mod + p.mod);
            }

        }




        private NodePositioning getPosDataFor(ReadonlyTreeNode<T> node)
        {
            return nodePositionings[node.NodeId];
        }








        //private void CalculateBaseXOfNodes()
        //{
        //    var root = tree.Root;

        //    TreeOperations.RunActionInPostOrder<TreeNodeAsset>(root, (node, h, x) =>
        //    {
        //        var nodeDrawer = new TreeNodeDrawer(node, x, h);
        //        nodeDrawers.Add(node, nodeDrawer);

        //        var children = new List<TreeNodeAsset>(node.Children);
        //        float desiredValue = x;
        //        if (children.Count == 1)
        //        {
        //            desiredValue = nodeDrawers[children[0]].localX;
        //        }
        //        else if (children.Count > 1)
        //        {
        //            var c1 = nodeDrawers[children[0]];
        //            var c2 = nodeDrawers[children[children.Count - 1]];
        //            desiredValue = (c2.localX - c1.localX) / 2;
        //        }

        //        var leftSibilings = new List<TreeNodeAsset>(node.SibilingsBefore);
        //        if (leftSibilings.Count == 0)
        //        {
        //            nodeDrawer.localX = desiredValue;
        //        }
        //        else
        //        {
        //            nodeDrawer.mod = nodeDrawer.localX - desiredValue;
        //        }
        //    });
        //}

        //private void ResolveNodeConflicts()
        //{
        //    var root = tree.Root;
        //    foreach (var node in TreeOperations.Preorder<TreeNodeAsset>(root))
        //    {
        //        var musicNode = (MusicTreeNode)node;

        //    }
        //}


        //private void CalculateLeftContour(MusicTreeNode node, ref Dictionary<int, float> result)
        //{
        //    var drawer = nodeDrawers[node];
        //    result.Add(drawer.height, CalculateRealX(node));

        //    var children = new List<MusicTreeNode>(node.GetChildrenAs<MusicTreeNode>());
        //    if (children.Count > 0)
        //    {
        //        var leftmost = children[0];
        //        CalculateLeftContour(leftmost, ref result);
        //    }
        //}
        //private void CalculateRightContour(MusicTreeNode node, ref Dictionary<int, float> result)
        //{
        //    var drawer = nodeDrawers[node];
        //    result.Add(drawer.height, CalculateRealX(node));

        //    var children = new List<MusicTreeNode>(node.GetChildrenAs<MusicTreeNode>());
        //    if (children.Count > 0)
        //    {
        //        var rightmost = children[children.Count - 1];
        //        CalculateRightContour(rightmost, ref result);
        //    }
        //}

        //private float CalculateRealX(MusicTreeNode node)
        //{
        //    return nodeDrawers[node].localX + CalculateRealX((MusicTreeNode)node.Parent);
        //}

        //private float CalculateMod(MusicTreeNode node)
        //{
        //    return nodeDrawers[node].mod + CalculateMod((MusicTreeNode)node.Parent);
        //}


        //private Vector2 CalculateSize()
        //{
        //    return new Vector2(900, 900);
        //}
    }
}
