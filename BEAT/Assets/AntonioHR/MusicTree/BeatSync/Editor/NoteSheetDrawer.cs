﻿using AntonioHR.MusicTree.BeatSync;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;
using AntonioHR.MusicTree.Editor;

namespace AntonioHR.MusicTree.BeatSync.Editor
{
    public class NoteSheetDrawer
    {
        private NoteSheet sheet;
        private List<NoteTrackDrawer> trackDrawers;
        private Vector2 scroll;


        public event Action DataUpdated;


        public NoteSheetDrawer(NoteSheet sheet)
        {
            this.sheet = sheet;
            trackDrawers = new List<NoteTrackDrawer>();
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
                return trackDrawers.Max(drawer => drawer.Width);
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

        public void Draw(Rect windowPos)
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
                    drawer.Draw();
                }
                cursor += new Vector2(0, rect.height + TrackSpacing);
            }
        }

        private void UpdateTrackDrawers()
        {
            trackDrawers.Clear();
            foreach (var track in sheet.tracks)
            {
                trackDrawers.Add(new NoteTrackDrawer(track, this));
            }
            if (DataUpdated != null)
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
