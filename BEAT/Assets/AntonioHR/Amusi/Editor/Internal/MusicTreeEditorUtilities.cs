using AntonioHR.Amusi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AntonioHR.Amusi.Data.Nodes;

namespace AntonioHR.Amusi.Editor.Internal
{
    public static class MusicTreeEditorUtilities
    {
        public static int BPMFor(CueMusicTreeNode node, MusicTreeAsset tree)
        {
            return node.useCustomBPM ? node.customBPM : tree.defaultBPM;
        }

        public static float DurationInBeats(CueMusicTreeNode node, MusicTreeAsset tree)
        {
            int bpm = BPMFor(node, tree);
            float result = node.clip.length / 60 * bpm;
            result = RoundBeat(result);
            return result;
        }

        public static float RoundBeat(float result, float minBeat = .25f, float threshold = .2f)
        {
            float f = result % minBeat;
            result -= f;
            if (f > threshold)
                result += minBeat;
            return result;
        }
    }
}
