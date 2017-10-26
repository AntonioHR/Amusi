using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AntonioHR.MusicTree.BeatSync.Editor
{
    public class NoteSheetEditorConfigs : ScriptableObject
    {
        [SerializeField]
        private int subTrackHeight = 50;
        [SerializeField]
        private int beatWidth;
        [SerializeField]
        private Texture bgTexture;
        [SerializeField]
        private Texture noteTextureInactive;
        [SerializeField]
        private Texture noteTextureActive;
        [SerializeField]
        private int trackSpacing = 5;
        [SerializeField]
        private GUISkin skin;
        [SerializeField]
        private int subTrackSpacing;

        public int SubTrackHeight
        {
            get
            {
                return subTrackHeight;
            }

            set
            {
                subTrackHeight = value;
            }
        }

        public int BeatWidth
        {
            get
            {
                return beatWidth;
            }

            set
            {
                beatWidth = value;
            }
        }

        public Texture BgTexture
        {
            get
            {
                return bgTexture;
            }

            set
            {
                bgTexture = value;
            }
        }

        public Texture NoteTextureInactive
        {
            get
            {
                return noteTextureInactive;
            }

            set
            {
                noteTextureInactive = value;
            }
        }

        public Texture NoteTextureActive
        {
            get
            {
                return noteTextureActive;
            }

            set
            {
                noteTextureActive = value;
            }
        }

        public int TrackSpacing
        {
            get
            {
                return trackSpacing;
            }
        }

        public GUISkin Skin
        {
            get
            {
                return skin;
            }
        }

        public int SubTrackSpacing
        {
            get { return subTrackSpacing; }
        }
    }
}
