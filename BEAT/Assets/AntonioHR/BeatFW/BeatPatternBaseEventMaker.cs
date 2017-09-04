using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AntonioHR.BeatFW
{
    public abstract class BeatPatternBaseEventMaker : MonoBehaviour
    {

        public BeatPattern pattern;

        BeatManager beatManager;

        float lastVal;

        public abstract void OnNote();

        public void Awake()
        {
            beatManager = FindObjectOfType<BeatManager>();
        }

        public void Update()
        {
            float curr = beatManager.BeatProgressFull;
            if (curr < 0)
                return;
            var diff = pattern.NotesBetween(lastVal, curr);
            for (int i = 0; i < diff; i++)
            {
                OnNote();
            }

            lastVal = curr;
        }
    }
}
