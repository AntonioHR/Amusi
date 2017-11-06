using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AntonioHR.MusicTree.Nodes;
using AntonioHR.MusicTree.BeatSync.Editor;
using System;
using AntonioHR.MusicTree.BeatSync;
using AntonioHR.MusicTree.Internal;
using UnityEditor;

namespace AntonioHR.MusicTree.Editor
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
        public PlayableRuntimeMusicTreeNode SelectedNode { get; private set; }
        public PlayableRuntimeMusicTreeNode NodeOfSelectedCue { get; private set; }
        public NoteSheet NoteSheet { get; private set; }
        public PlayableRuntimeMusicTree CachedTree { get; private set; }

        public MusicTreePlayer Player { get; private set; }
        public CueMusicTreeNode PlayedNode { get { return Player == null ? null : Player.CurrentNode; } }


        //Events
        public event Action NoteTrackDefinitionsChanged;
        public event Action<MusicTreeAsset> SelectedTreeAssetChanged;
        public event Action<PlayableRuntimeMusicTree> TreeHierarchyChanged;
        public event Action<PlayableRuntimeMusicTreeNode> SelectedNodeChanged;

        public event Action<CueMusicTreeNode, PlayableRuntimeMusicTreeNode> SelectedCueChanged;
        public event Action<NoteSheetEditorWindow> NoteSheetEditorOpened;
        public event Action<MusicTreeEditorWindow> MusicTreeEditorOpened;
        
        

        private MusicTreeEditorManager()
        {
            if (MusicTreePlayer.Instance != null)
                OnTreePlayerChanged();
            MusicTreePlayer.InstanceChanged += OnTreePlayerChanged;
        }

        private void OnTreePlayerChanged()
        {
            if(Player != null)
                Player.NewNodePlaying -= OnNewNodePayingByPlayer;
            Player = MusicTreePlayer.Instance;
            if (Player != null)
                Player.NewNodePlaying += OnNewNodePayingByPlayer;
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
                CachedTree = PlayableRuntimeMusicTree.CreateFrom(TreeAsset);
            } else
            {
                CachedTree = null;
            }
            TreeHierarchyChanged(CachedTree);
        }
        public void OnNodeSelected(PlayableRuntimeMusicTreeNode n)
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
        private void OnCueSelected(CueMusicTreeNode cue, PlayableRuntimeMusicTreeNode owner)
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