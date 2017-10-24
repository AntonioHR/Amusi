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
        private int subTrackHeight;
        [SerializeField]
        private int beatWidth;
        [SerializeField]
        private Texture bgTexture;
        [SerializeField]
        private Texture noteTextureInactive;
        [SerializeField]
        private Texture noteTextureActive;

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
    }
}
