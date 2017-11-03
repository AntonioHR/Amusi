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

        [Space()]
        [SerializeField]
        private GUISkin skin;

        public Texture ConditionIcon { get { return conditionIcon; } }
        public Texture CueIcon { get { return cueIcon; } }
        public Texture SelectorIcon { get { return selectorIcon; } }
        public Texture SequenceIcon { get { return sequenceIcon; } }


        public Texture DragIndicatorIcon { get { return dragIndicatorIcon; } }
        public Texture NodeUnselected { get { return nodeUnselected; } }
        public Texture NodeSelected { get { return nodeSelected; } }
        public Texture NodeDropTarget { get { return nodeDropTarget; } }

        public GUISkin Skin { get { return skin; } }
    }
}
