using AntonioHR.MusicTree.Nodes;
using AntonioHR.TreeAsset;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AntonioHR.MusicTree.Visualizer.Editor
{
    public class MusicTreeVisualizerWindow : EditorWindow
    {
        public MusicTreeAsset tree;

        public Vector2 scrollPos;

        TreeDrawer drawer;

        [MenuItem("Window/MusicTree Visualizer")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow(typeof(MusicTreeVisualizerWindow));
        }

        void OnGUI()
        {
            EditorGUI.BeginChangeCheck();

            tree = EditorGUILayout.ObjectField(tree, typeof(MusicTreeAsset)) as MusicTreeAsset;
            if (EditorGUI.EndChangeCheck())
            {
                //Changed
                drawer = tree != null? new TreeDrawer(tree): null;
            }


            using(var scrollview = new EditorGUILayout.ScrollViewScope(scrollPos))
            {
                //GUILayoutUtility.GetRect(900, 900);
                //EditorGUI.DrawRect(new Rect(100, 100, 500, 300), Color.red);
                if(drawer != null)
                {
                    drawer.Draw();
                }
                scrollPos = scrollview.scrollPosition;
            }
        }
    }

    public class TreeDrawer
    {

        public MusicTreeAsset tree;

        public int sibilingSeparation = 120;
        public int subTreeSeparation = 140;

        public int levelSeparation = 100;

        public int yStart = 100;
        public int xStart = 100;

        public int nodeSize = 50;


        Texture sequence_icon;
        Texture selector_icon;
        Texture cue_icon;



        Dictionary<TreeNodeAsset, TreeNodeDrawer> nodeDrawers;

        class TreeNodeDrawer
        {
            public TreeNodeDrawer(TreeNodeAsset node, float localX, int height)
            {
                this.node = node;
                this.localX = localX;
                this.height = height;
                this.mod = 0;
            }
            public TreeNodeAsset node;
            public float localX;
            public int height;
            public float mod;
        }

        public TreeDrawer(MusicTreeAsset tree)
        {
            this.tree = tree;
            nodeDrawers = new Dictionary<TreeNodeAsset, TreeNodeDrawer>();

            selector_icon =  Resources.Load<Texture>("icon_selector");
            sequence_icon = Resources.Load<Texture>("icon_sequence");
            cue_icon = Resources.Load<Texture>("icon_music");
        }

        public void Draw()
        {
            nodeDrawers = new Dictionary<TreeNodeAsset, TreeNodeDrawer>();


            calculateBaseXOfNodes();

            //resolveNodeConflicts();

            reserveLayoutSpace();

            DrawNodes();
        }

        private void DrawNodes()
        {
            DrawNodeRecursion(tree.Root, 0, Vector2.zero);
        }

        private void DrawNodeRecursion(TreeNodeAsset node, float mod, Vector2 parentPos)
        {
            var nodeDrawer = nodeDrawers[node];
            Rect baseRect = new Rect(0, 0, nodeSize, nodeSize);
            var nodePosition = new Vector2((nodeDrawer.localX + mod) * sibilingSeparation + xStart, 
                nodeDrawer.height * levelSeparation + yStart);



            foreach (var child in node.Children)
            {
                DrawNodeRecursion(child, mod + nodeDrawer.mod, nodePosition);
            }

            if (!node.IsRoot)
            {
                var offset = (Vector2.right * .5f * nodeSize);
                Handles.DrawLine(nodePosition + offset, parentPos + offset);
            }
            DrawNode(ref baseRect, ref nodePosition, node);
        }

        private void DrawNode(ref Rect baseRect, ref Vector2 nodePosition, TreeNodeAsset node)
        {
            Color color = Color.gray;

            Texture tex = null;

            if(node is CueMusicTreeNode)
            {
                //color = Color.red;
                tex = cue_icon;
            }
            else if (node is SelectorMusicTreeNode)
            {
                //color = Color.green;
                tex = selector_icon;
            }
            else if (node is SequenceMusicTreeNode)
            {
                //color = Color.cyan;
                tex = sequence_icon;
            } 


            EditorGUI.DrawRect(baseRect.At(nodePosition), color);
            if (tex != null)
                GUI.DrawTexture(baseRect.At(nodePosition).Resized(Vector2.one * 1), tex);

            if(GUI.Button(baseRect.At(nodePosition), GUIContent.none, GUIStyle.none))
            {
                Selection.activeObject = node;
            }
        }


        private void calculateBaseXOfNodes()
        {
            var root = tree.Root;

            TreeOperations.RunActionInPostOrder<TreeNodeAsset>(root, (node, h, x)=>
                {
                    var nodeDrawer = new TreeNodeDrawer(node, x, h);
                    nodeDrawers.Add(node, nodeDrawer);

                    var children = new List<TreeNodeAsset>(node.Children);
                    float desiredValue = x;
                    if(children.Count == 1)
                    {
                        desiredValue = nodeDrawers[children[0]].localX;
                    } 
                    else if (children.Count > 1)
                    {
                        var c1 = nodeDrawers[children[0]];
                        var c2 = nodeDrawers[children[children.Count-1]];
                        desiredValue = (c2.localX - c1.localX) / 2;
                    }

                    var leftSibilings = new List<TreeNodeAsset>(node.SibilingsBefore);
                    if (leftSibilings.Count == 0)
                    {
                        nodeDrawer.localX = desiredValue;
                    }
                    else
                    {
                        nodeDrawer.mod = nodeDrawer.localX - desiredValue;
                    }
                });
        }

        private void resolveNodeConflicts()
        {
            throw new System.NotImplementedException();
        }


        private void reserveLayoutSpace()
        {
            Vector2 size = calculateSize();
            GUILayoutUtility.GetRect(size.x, size.y);
        }

        private Vector2 calculateSize()
        {
            return new Vector2(900, 900);
        }
    }
}