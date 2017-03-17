using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BeatFW
{
    public class BeatManager:MonoBehaviour
    {
        public DummyPatchSelector selector;
        
        void Start()
        {
            Debug.Log("Initializing");
        }
    }
}
