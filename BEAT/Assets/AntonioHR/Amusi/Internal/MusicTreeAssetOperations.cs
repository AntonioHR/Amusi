using AntonioHR.Amusi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntonioHR.Amusi.Internal
{
    public static class MusicTreeAssetOperations
    {
        public static void DeleteTrack(this CachedMusicTree cachedTree, NoteTrackDefinition def)
        {
            var trackDefs = cachedTree.Asset.trackDefinitions;
            int index = trackDefs.IndexOf(def);

            if (index == -1)
                throw new Exception("Track def is not owned by tree");

            foreach (var cueNode in cachedTree.AllCues)
            {
                cueNode.Tracks.RemoveAt(index);
            }
            trackDefs.RemoveAt(index);
        }

        public static void CreateTrack(this CachedMusicTree cachedTree, string name)
        {
            var trackDefs = cachedTree.Asset.trackDefinitions;
            trackDefs.Add(new NoteTrackDefinition() { name = name });

            foreach (var cueNode in cachedTree.AllCues)
            {
                cueNode.sheet.tracks.Add(new NoteTrack() { name = name, notes  = new List<Note>() });
                UnityEngine.Debug.Assert(cueNode.Tracks.Count == trackDefs.Count);
            }
            
        }
    }
        
}
