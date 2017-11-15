using AntonioHR.Amusi.Data;
using AntonioHR.Amusi.Data.Nodes;
using AntonioHR.Amusi.Editor.Window.NoteSheet;
using AntonioHR.Amusi.Editor.Windows.MusicTree;
using AntonioHR.Amusi.Internal;
using System;
using UnityEditor;

namespace AntonioHR.Amusi.Editor
{
    public class MusicTreeEditorManager {
        public static MusicTreeEditorManager Instance { get
            {
                if(instance == null)
                {
                    instance = new MusicTreeEditorManager();
                }
                return instance;
            }
        }

        private static MusicTreeEditorManager instance;
        
        //Properties
        public NoteSheetEditorWindow NoteSheetEditor { get; private set; }
        public MusicTreeEditorWindow MusicTreeEditor { get; private set; }
        public MusicTreeAsset TreeAsset { get; private set; }
        public CachedMusicTreeNode SelectedNode { get; private set; }
        public CachedMusicTreeNode NodeOfSelectedCue { get; private set; }
        public NoteSheet NoteSheet { get; private set; }
        public CachedMusicTree CachedTree { get; private set; }

        public AmusiEngine Engine { get; private set; }
        public CueMusicTreeNode PlayedNode { get { return Engine == null ? null : Engine.CurrentNode; } }


        //Events
        public event Action NoteTrackDefinitionsChanged;
        public event Action<MusicTreeAsset> SelectedTreeAssetChanged;
        public event Action<CachedMusicTree> TreeHierarchyChanged;
        public event Action<CachedMusicTreeNode> SelectedNodeChanged;

        public event Action<CueMusicTreeNode, CachedMusicTreeNode> SelectedCueChanged;
        public event Action<NoteSheetEditorWindow> NoteSheetEditorOpened;
        public event Action<MusicTreeEditorWindow> MusicTreeEditorOpened;
        
        

        private MusicTreeEditorManager()
        {
            if (AmusiEngine.Instance != null)
                OnTreePlayerChanged();
            AmusiEngine.InstanceChanged += OnTreePlayerChanged;
        }


        private void OnTreePlayerChanged()
        {
            if(Engine != null)
                Engine.NewNodePlaying -= OnNewNodePayingByPlayer;
            Engine = AmusiEngine.Instance;
            if (Engine != null)
                Engine.NewNodePlaying += OnNewNodePayingByPlayer;
        }
        private void OnNewNodePayingByPlayer(CueMusicTreeNode obj)
        {
            if (MusicTreeEditor != null)
                MusicTreeEditor.Repaint();
        }


        public void OnSelectedTreeChanged(MusicTreeAsset a)
        {
            TreeAsset = a;
            if (SelectedTreeAssetChanged != null)
                SelectedTreeAssetChanged(TreeAsset);
            OnChangesToTreeHierarchy();
        }
        public void OnChangesToTreeHierarchy()
        {
            if(TreeAsset != null)
            {
                CachedTree = CachedMusicTree.CreateFrom(TreeAsset);
            } else
            {
                CachedTree = null;
            }
            TreeHierarchyChanged(CachedTree);
        }
        public void OnNodeSelected(CachedMusicTreeNode n)
        {
            SelectedNode = n;
            if(n != null)
                Selection.activeObject = n.Asset;
            if (SelectedNodeChanged != null)
                SelectedNodeChanged(SelectedNode);

            var cue = n.Asset as CueMusicTreeNode;
            if(cue != null)
            {
                OnCueSelected(cue, n);
            }
        }
        private void OnCueSelected(CueMusicTreeNode cue, CachedMusicTreeNode owner)
        {
            if(SelectedCueChanged != null)
            {
                SelectedCueChanged(cue, owner);
            }
        }
        public void OnNoteSheetEditorOpened(NoteSheetEditorWindow e)
        {
            NoteSheetEditor = e;
            if(NoteSheetEditorOpened != null)
                NoteSheetEditorOpened(NoteSheetEditor);
        }
        public void OnMusicTreeEditorOpened(MusicTreeEditorWindow e)
        {
            MusicTreeEditor = e;
            if(MusicTreeEditorOpened != null)
                MusicTreeEditorOpened(MusicTreeEditor);
        }

        


    }
}