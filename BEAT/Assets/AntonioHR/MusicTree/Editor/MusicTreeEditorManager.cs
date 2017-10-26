using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AntonioHR.MusicTree.Nodes;
using AntonioHR.MusicTree.BeatSync.Editor;
using System;
using AntonioHR.MusicTree.BeatSync;

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
        public int BeatsPerMeasure
        {
            get
            {
                Debug.LogWarning("No Beats Per Measure Calculated");
                return 4;
            }
        }
        public int SubtrackCount
        {
            get
            {
                Debug.LogWarning("No Beats Per Measure Calculated");
                return 5;
            }
        }
        public int MeasureCount
        {
            get
            {
                Debug.LogWarning("No Beats Per Measure Calculated");
                return 4;
            }
        }

        public NoteSheetEditorWindow NoteSheetEditor { get; private set; }
        public MusicTreeVisualizerWindow MusicTreeEditor { get; private set; }
        public MusicTreeAsset TreeAsset { get; private set; }
        public MusicTreeNode SelectedNode { get; private set; }
        public NoteSheet NoteSheet { get; private set; }

        private static MusicTreeEditorManager instance;

        public event Action NoteTrackDefinitionsChanged;
        public event Action<MusicTreeAsset> SelectedTreeChanged;
        public event Action<MusicTreeNode> SelectedNodeChanged;
        public event Action<NoteSheetEditorWindow> NoteSheetEditorOpened;
        public event Action<MusicTreeVisualizerWindow> MusicTreeEditorOpened;
        
        

        private MusicTreeEditorManager()
        {

        }
        
        public void OnTreeChanged(MusicTreeAsset a)
        {
            TreeAsset = a;
            if (SelectedTreeChanged != null)
                SelectedTreeChanged(TreeAsset);
        }

        public void OnNodeSelected(MusicTreeNode n)
        {
            SelectedNode = n;
            if(SelectedNodeChanged != null)
                SelectedNodeChanged(SelectedNode);
        }

        public void OnNoteSheetEditorOpened(NoteSheetEditorWindow e)
        {
            NoteSheetEditor = e;
            if(NoteSheetEditorOpened != null)
                NoteSheetEditorOpened(NoteSheetEditor);
        }
        
        public void OnMusicTreeEditorOpened(MusicTreeVisualizerWindow e)
        {
            MusicTreeEditor = e;
            if(MusicTreeEditorOpened != null)
                MusicTreeEditorOpened(MusicTreeEditor);
        }
        


    }
}