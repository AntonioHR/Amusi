﻿using AntonioHR.MusicTree.Nodes;
using AntonioHR.MusicTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.AntonioHR.MusicTree.Editor.Internal
{
    public static class MusicTreeEditorUitilities
    {
        public static int BPMFor(CueMusicTreeNode node, MusicTreeAsset tree)
        {
            return node.useCustomBPM ? node.customBPM : tree.defaultBPM;
        }

        public static float DurationInBeats(CueMusicTreeNode node, MusicTreeAsset tree)
        {
            int bpm = BPMFor(node, tree);
            float result = node.clip.length / 60 * bpm;
            float f = result % .25f;
            result -= f;
            if (f > .2)
                result += .25f;
            return result;
        }
    }
}
