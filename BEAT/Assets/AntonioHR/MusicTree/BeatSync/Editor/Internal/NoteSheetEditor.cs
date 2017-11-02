using AntonioHR.MusicTree.BeatSync;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;
using AntonioHR.MusicTree.Editor;
using AntonioHR.MusicTree.Internal;
using AntonioHR.MusicTree.Nodes;

namespace AntonioHR.MusicTree.BeatSync.Editor.Internal
{
    public class NoteSheetEditor
    {
        private NoteSheet Sheet { get { return cue.sheet; } }
        private PlayableRuntimeMusicTreeNode cueOwner;
        private CueMusicTreeNode cue;
        private List<NoteTrackEditor> trackDrawers;
        private Vector2 scroll;
        
        public int NotesPerBar { get { return cueOwner.Tree.Asset.NotesPerBar; } }

        public event Action DataUpdated;


        public NoteSheetEditor(CueMusicTreeNode cue, PlayableRuntimeMusicTreeNode cueOwner)
        {
            this.cue = cue;
            this.cueOwner = cueOwner;
            trackDrawers = new List<NoteTrackEditor>();
            MusicTreeEditorManager.Instance.NoteTrackDefinitionsChanged += Instance_NoteTrackDefinitionsChanged;
            UpdateTrackDrawers();
        }

        public int TotalHeight
        {
            get
            {
                return trackDrawers.Aggregate(0, (sum, drawer) => sum + drawer.Height) + TrackSpacing * (trackDrawers.Count - 1);
            }
        }
        public int TotalWidth
        {
            get
            {
                return trackDrawers.Count == 0? 0: trackDrawers.Max(drawer => drawer.Width);
            }
        }
        public int TrackSpacing
        {
            get
            {
                return NoteSheetEditorWindow.configs.TrackSpacing;
            }
        }
        public Vector2 Border
        {
            get
            {
                return new Vector2(10, 10);
            }
        }

        public float CueLengthInBeats { get { return cueOwner.LengthInBeats; } }

        public void Update(Rect windowPos)
        {
            if (trackDrawers.Count == 0)
            {
                GUI.Label(new Rect(0, 0, 100, 20), "No Tracks to draw");
            }
            Rect view = new Rect(Vector2.zero, windowPos.size);
            Rect totalSize = new Rect(Vector2.zero, new Vector2(TotalWidth, TotalHeight) + Border * 2);
            using (var scrollScope = new GUI.ScrollViewScope(view, scroll, totalSize))
            {
                scroll = scrollScope.scrollPosition;
                DrawTracks(windowPos);
            }
        }

        public void OnReplaced()
        {
            MusicTreeEditorManager.Instance.NoteTrackDefinitionsChanged -= Instance_NoteTrackDefinitionsChanged;
        }
        


        private void DrawTracks(Rect windowPos)
        {
            Vector2 cursor = Border;
            foreach (var drawer in trackDrawers)
            {

                var size = new Vector2(drawer.Width, drawer.Height);

                var rect = new Rect(cursor, size);
                
                using (var groupScope = new GUI.GroupScope(rect))
                {
                    drawer.Update();
                }
                cursor += new Vector2(0, rect.height + TrackSpacing);
            }
        }

        private void UpdateTrackDrawers()
        {
            foreach (var dr in trackDrawers)
            {

                dr.DataUpdated -= TrackDrawer_DataUpdated;
            }

            trackDrawers.Clear();
            foreach (var track in Sheet.tracks)
            {
                var dr = new NoteTrackEditor(track, this);
                dr.DataUpdated += TrackDrawer_DataUpdated;
                trackDrawers.Add(dr);
            }
            if (DataUpdated != null)
                DataUpdated();
        }

        private void TrackDrawer_DataUpdated()
        {
            DataUpdated();
        }



        #region Event Handlers
        private void Instance_NoteTrackDefinitionsChanged()
        {
            UpdateTrackDrawers();
        }
        #endregion
    }
}
