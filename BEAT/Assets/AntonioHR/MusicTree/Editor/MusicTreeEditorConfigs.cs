using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AntonioHR.MusicTree.Editor
{
    public class MusicTreeEditorConfigs : ScriptableObject
    {
        [SerializeField]
        private Texture conditionIcon;
        [SerializeField]
        private Texture cueIcon;
        [SerializeField]
        private Texture selectorIcon;
        [SerializeField]
        private Texture sequenceIcon;

        [Space()]
        [SerializeField]

        private Texture dragIndicatorIcon;
        [Space()]
        [SerializeField]
        private Texture nodeUnselected;
        [SerializeField]
        private Texture nodeSelected;
        [SerializeField]
        private Texture nodeDropTarget;
        [SerializeField]
        private Texture nodeDropTargetUnable;
        [SerializeField]
        private Texture nodePlayed;

        [Space()]
        [SerializeField]
        private GUISkin skin;

        public Texture ConditionIcon { get { return conditionIcon; } }
        public Texture CueIcon { get { return cueIcon; } }
        public Texture SelectorIcon { get { return selectorIcon; } }
        public Texture SequenceIcon { get { return sequenceIcon; } }


        public Texture DragIndicatorIcon { get { return dragIndicatorIcon; } }

        public Texture NodePlayed { get { return nodePlayed; } }
        public Texture NodeUnselected { get { return nodeUnselected; } }
        public Texture NodeSelected { get { return nodeSelected; } }
        public Texture NodeDropTarget { get { return nodeDropTarget; } }
        public Texture NodeDropTargetUnable { get { return nodeDropTargetUnable; } }

        public GUISkin Skin { get { return skin; } }
    }
}
