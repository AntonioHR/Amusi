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

        public Texture ConditionIcon { get { return conditionIcon; } }
        public Texture CueIcon { get { return cueIcon; } }
        public Texture SelectorIcon { get { return selectorIcon; } }
        public Texture SequenceIcon { get { return sequenceIcon; } }
    }
}
