using AntonioHR.Amusi.Data;
using AntonioHR.Amusi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntonioHR.Amusi.Data.Nodes
{
    public static class MusicTreeNodeUtilities
    {
        public static int BPMFor(CueMusicTreeNode node, MusicTreeAsset tree)
        {
            return node.useCustomBPM ? node.customBPM : tree.defaultBPM;
        }

        public static float DurationInBeats(CueMusicTreeNode node, MusicTreeAsset tree)
        {
            int bpm = BPMFor(node, tree);
            float result = node.clip == null ? 0 : node.clip.length / 60 * bpm;
            float f = result % .25f;
            result -= f;
            if (f > .125)
                result += .25f;
            return result;
        }
    }
}
