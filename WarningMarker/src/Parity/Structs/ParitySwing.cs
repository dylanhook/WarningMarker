using System.Collections.Generic;
using UnityEngine;

namespace WarningMarker.Parity.Structs
{
    public class ParitySwing
    {
        public List<ParityNote> Notes = new List<ParityNote>(4);
        public bool IsRightHand;
        public float StartBeat;
        public float EndBeat;
        public Vector2 StartPos;
        public Vector2 EndPos;
        public float StartAngle;
        public float EndAngle;
        public bool IsReset;
        public Parity Parity;

        public void AddNote(ParityNote note)
        {
            Notes.Add(note);
        }
    }
}