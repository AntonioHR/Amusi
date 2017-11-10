using AntonioHR.Amusi.Data;
using AntonioHR.Amusi.Data.Nodes;
using AntonioHR.Amusi.Editor.Internal;
using AntonioHR.Amusi.Internal;
using AntonioHR.TreeAsset;
using AntonioHR.TreeAsset.Internal;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace AntonioHR.Amusi.Editor.Windows.MusicTree.Internal
{
    public class MusicTreeNodePositioning : TreeNodePositioning<MusicTreeNodePositioning, CachedMusicTree, MusicTreeAsset, MusicTreeNode, CachedMusicTreeNode>
    {
    }
    public class MusicTreeEditor
    {

        #region Drawing Parameters

        Texture SequenceIcon { get { return MusicTreeEditorWindow.configs.SequenceIcon; } }
        Texture SelectorIcon { get { return MusicTreeEditorWindow.configs.SelectorIcon; } }
        Texture CueIcon { get { return MusicTreeEditorWindow.configs.CueIcon; } }
        Texture ConditionIcon { get { return MusicTreeEditorWindow.configs.ConditionIcon; } }
        #endregion


        CachedMusicTree tree;
        MusicTreeNodePositioning cachedPositioning;


        //State
        CachedMusicTreeNode selection;

        CachedMusicTreeNode dropTarget;
        int dropIndex;
        bool isDropValid;

        public MusicTreeEditor(CachedMusicTree tree)
        {
            this.tree = tree;
            
            cachedPositioning = MusicTreeNodePositioning.CreateFrom(tree);
        }
        
        public void Update()
        {
            int id = GUIUtility.GetControlID(FocusType.Passive);

            switch (Event.current.GetTypeForControl(id))
            {
                case EventType.MouseDown:
                    OnMouseDown();
                    break;
                case EventType.MouseUp:
                    OnMouseUp();
                    break;
                case EventType.MouseDrag:
                    OnMouseDrag();
                    break;
                case EventType.Repaint:
                    OnRepaint();
                    break;
                case EventType.Layout:
                    OnLayout();
                    break;
                case EventType.ContextClick:
                    break;
                default:
                    break;
            }
        }

        private void OnMouseUp()
        {
            Event.current.Use();
            if(dropTarget != null && selection != null && isDropValid)
            {
                MoveNode(selection, dropTarget, dropIndex);
            }
            dropTarget = null;
        }

        private void MoveNode(CachedMusicTreeNode selection, CachedMusicTreeNode dropTarget, int dropIndex)
        {
            selection.Asset.ChangeParentTo(dropTarget.Asset, dropIndex);
            MusicTreeEditorManager.Instance.OnChangesToTreeHierarchy();
        }

        private void OnMouseDrag()
        {
            dropTarget = null;
            if (selection == null)
                return;
            foreach (var node in tree.AllNodes)
            {
                if (node == selection)
                    continue;

                var bounds = cachedPositioning.GetBoundsFor(node);
                if (bounds.Contains(Event.current.mousePosition))
                {
                    dropTarget = node;
                    dropIndex = dropTarget.ChildCount;
                    isDropValid = node.CanBeParentOf(selection);
                    break;
                }
                var childrenBounds = cachedPositioning.GetDropBoundsFor(node);
                for (int i = 0; i < childrenBounds.Length; i++)
                {
                    if(childrenBounds[i].Contains(Event.current.mousePosition))
                    {
                        dropTarget = node;
                        dropIndex = i;
                        isDropValid = node.CanBeParentOf(selection);
                        break;
                    }
                }
            }
            Event.current.Use();
        }

        private void OnMouseDown()
        {
            CachedMusicTreeNode hitNode = null;
            foreach (var node in tree.AllNodes)
            {
                var bounds = cachedPositioning.GetBoundsFor(node);
                if(bounds.Contains(Event.current.mousePosition))
                {
                    hitNode = node;
                    break;
                }
            }
            if(Event.current.button == 0)
            {
                if (hitNode != null)
                {
                    selection = hitNode;
                    MusicTreeEditorManager.Instance.OnNodeSelected(hitNode);
                }
            } else
            {
                if (hitNode != null)
                    OpenNodeContextMenu(hitNode);
            }
            Event.current.Use();
        }

        private void OpenNodeContextMenu(CachedMusicTreeNode node)
        {
            GenericMenu menu = new GenericMenu();
            if(node.AllowsMoreChildren)
            {
                menu.AddItem(new GUIContent("Add Cue Node"), false, () =>AddChild<CueMusicTreeNode>(node));
                menu.AddItem(new GUIContent("Add Selector Node"), false, () => AddChild<SelectorMusicTreeNode>(node));
                menu.AddItem(new GUIContent("Add Sequence Node"), false, () => AddChild<SequenceMusicTreeNode>(node));
                menu.AddItem(new GUIContent("Add Condition Node"), false, () => AddChild<ConditionMusicTreeNode>(node));
            }
            menu.AddItem(new GUIContent("Delete"), false, () => TryRemoveNode(node));
            menu.ShowAsContext();
        }

        private void AddChild<T>(CachedMusicTreeNode node) where T: MusicTreeNode
        {
            node.Tree.Asset.CreateChildFor<T>(node.Asset);
            MusicTreeEditorManager.Instance.OnChangesToTreeHierarchy();
        }

        private void TryRemoveNode(CachedMusicTreeNode node)
        {
            node.Tree.Asset.DeleteNodeAndAllChildren(node.Asset);
            MusicTreeEditorManager.Instance.OnChangesToTreeHierarchy();
        }

        private void OnRepaint()
        {
            foreach (var node in tree.AllNodes)
            {
                if (!node.IsRoot)
                {
                    DrawLineToParent(node, node.Asset);
                }
            }

            foreach (var node in tree.AllNodes)
            {
                DrawNode(cachedPositioning.GetBoundsFor(node), node);
            }

            if(dropTarget != null && isDropValid)
            {
                DrawDropTarget();
            }
        }

        private void DrawDebugDropTargets()
        {
            Color c1 = new Color(0, 0, 1, .3f);
            Color c2 = new Color(1, 0, 0, .3f);
            int max = dropTarget.ChildCount;
            int i = 0;
            foreach (var rect in cachedPositioning.GetDropBoundsFor(dropTarget).Reverse())
            {
                EditorGUI.DrawRect(rect, Color.Lerp(c1, c2, i / (float)max));
                i++;
            }
        }

        private void DrawDropTarget()
        {
            var bounds = cachedPositioning.GetBoundsFor(dropTarget);
            Vector2 indicatorPos = cachedPositioning.GetDropIndicatorPosFor(dropTarget, dropIndex);
            

            GUI.DrawTexture(bounds.CenteredAt(indicatorPos), MusicTreeEditorWindow.configs.DragIndicatorIcon);
        }

        private void DrawLineToParent(CachedMusicTreeNode node, MusicTreeNode musicTreeNode)
        {
            var parentBounds = cachedPositioning.GetBoundsFor(node.Parent);
            var myBounds = cachedPositioning.GetBoundsFor(node);

            Handles.DrawLine(parentBounds.center, myBounds.center);
        }
        private void DrawNode(Rect bounds, CachedMusicTreeNode node)
        {

            GUI.DrawTexture(bounds, BGTexFor(node));

            Texture tex = IconTextFor(node);
            if (tex != null)
                GUI.DrawTexture(bounds.Resized(Vector2.one * 1), tex);
        }
        private Texture IconTextFor(CachedMusicTreeNode node)
        {
            Texture tex = null;

            if (node.Asset is CueMusicTreeNode)
            {
                //color = Color.red;
                tex = CueIcon;
            }
            else if (node.Asset is SelectorMusicTreeNode)
            {
                //color = Color.green;
                tex = SelectorIcon;
            }
            else if (node.Asset is SequenceMusicTreeNode)
            {
                //color = Color.cyan;
                tex = SequenceIcon;
            }
            else if (node.Asset is ConditionMusicTreeNode)
            {
                tex = ConditionIcon;
            }

            return tex;
        }
        private Texture BGTexFor(CachedMusicTreeNode node)
        {
            if (node.Asset == MusicTreeEditorManager.Instance.PlayedNode)
                return MusicTreeEditorWindow.configs.NodePlayed;
            if (selection == node)
                return MusicTreeEditorWindow.configs.NodeSelected;
            else if (dropTarget == node)
            {
                if(isDropValid)
                    return MusicTreeEditorWindow.configs.NodeDropTarget;
                else
                    return MusicTreeEditorWindow.configs.NodeDropTargetUnable;
            }
            else
                return MusicTreeEditorWindow.configs.NodeUnselected;
        }

        private void OnLayout()
        {
            Vector2 size = cachedPositioning.GetTreeSize();
            GUILayoutUtility.GetRect(size.x, size.y);
        }

    }
}
