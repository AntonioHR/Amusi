using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AntonioHR.MusicTree.Nodes;
using AntonioHR.MusicTree.BeatSync.Editor;
using System;

namespace AntonioHR.MusicTree.Editor
{
    public class MusicTreeEditorManager {
        public static MusicTreeEditorManager Instance { get; private set; }
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

        NoteSheetEditorWindow noteSheetEditor;
        MusicTreeVisualizerWindow musicTreeEditor;
        MusicTreeAsset treeAsset;
        MusicTreeNode selectedNode;

        public event Action<MusicTreeAsset> SelectedTreeChanged;
        public event Action<MusicTreeNode> SelectedNodeChanged;
        public event Action<NoteSheetEditorWindow> NoteSheetEditorOpened;
        public event Action<MusicTreeVisualizerWindow> MusicTreeEditorOpened;


        public void OnTreeChanged(MusicTreeAsset asset)
        {
            treeAsset = asset;
            if (SelectedTreeChanged != null)
                SelectedTreeChanged(treeAsset);
        }

        public void OnNodeSelected(MusicTreeNode node)
        {
            node = selectedNode;
            if(SelectedNodeChanged != null)
                SelectedNodeChanged(node);
        }

        public void OnNoteSheetEditorOpened(NoteSheetEditorWindow editor)
        {
            noteSheetEditor = editor;
            if(NoteSheetEditorOpened != null)
                NoteSheetEditorOpened(editor);
        }
        
        public void OnMusicTreeEditorOpened(MusicTreeVisualizerWindow editor)
        {
            musicTreeEditor = editor;
            if(MusicTreeEditorOpened != null)
                MusicTreeEditorOpened(editor);
        }


    }
}