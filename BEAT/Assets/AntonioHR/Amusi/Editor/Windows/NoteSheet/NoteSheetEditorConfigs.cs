using UnityEngine;

namespace AntonioHR.Amusi.Editor.Window.NoteSheet
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
        private Texture ghostNoteTexture;
        [SerializeField]
        private int trackSpacing = 5;
        [SerializeField]
        private GUISkin skin;
        [SerializeField]
        private int subTrackSpacing;
        [SerializeField]
        private float noteInsertionSnap = .5f;
        [SerializeField]
        private float noteResizeSnap = .25f;
        [SerializeField]
        private float noteMoveSnap = .25f;
        [SerializeField]
        private float defaultNoteSize = 1.0f;
        [SerializeField]
        private int noteResizeBorderWidth = 5;

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

        public Texture GhostNoteTexture
        {
            get
            {
                return ghostNoteTexture;
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

        public float NoteInsertionSnap
        {
            get
            {
                return noteInsertionSnap;
            }
        }
        public float NoteResizeSnap
        {
            get
            {
                return noteResizeSnap;
            }
        }
        public float NoteMoveSnap
        {
            get
            {
                return noteMoveSnap;
            }
        }

        public float DefaultNoteSize
        {
            get
            {
                return defaultNoteSize;
            }
        }
        public int NoteResizeBorderWidth
        {
            get
            {
                return noteResizeBorderWidth;
            }
        }

    }
}
