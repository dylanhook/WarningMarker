using System.Collections.Generic;
using WarningMarker.Parity.Structs;
using UnityEngine;

namespace WarningMarker.Parity
{
    public class ParityCheck
    {
        public void Analyze(List<ParitySwing> swings)
        {
            if (swings.Count == 0) return;

            var firstSwing = swings[0];
            var firstNote = firstSwing.Notes[0];

            firstSwing.Parity = IsForehandStart(firstNote.CutDirection) ? Parity.Forehand : Parity.Backhand;
            CalculateSwingAngles(firstSwing);

            for (int i = 1; i < swings.Count; i++)
            {
                var prevSwing = swings[i - 1];
                var currentSwing = swings[i];

                var expectedParity = prevSwing.Parity == Parity.Forehand ? Parity.Backhand : Parity.Forehand;
                if (IsReset(prevSwing, currentSwing))
                {
                    currentSwing.IsReset = true;

                    currentSwing.Parity = prevSwing.Parity;
                }
                else
                {
                    currentSwing.Parity = expectedParity;
                }

                CalculateSwingAngles(currentSwing);
            }
        }

        private bool IsForehandStart(int cutDir)
        {
            return cutDir == 1 || cutDir == 6 || cutDir == 7;
        }

        private void CalculateSwingAngles(ParitySwing swing)
        {
            var note = swing.Notes[0];
            var dict = swing.Parity == Parity.Forehand ? ParityUtils.ForehandDict : ParityUtils.BackhandDict;

            if (dict.TryGetValue(note.CutDirection, out float angle))
            {
                swing.StartAngle = angle;
                swing.EndAngle = angle;
            }
            else
            {
                swing.StartAngle = 0;
                swing.EndAngle = 0;
            }
        }

        private bool IsReset(ParitySwing prev, ParitySwing curr)
        {
            var currNote = curr.Notes[0];
            if (currNote.CutDirection == 8) return false;

            float currentAFN = (prev.Parity != Parity.Forehand) ?
                ParityUtils.BackhandDict[prev.Notes[0].CutDirection] :
                ParityUtils.ForehandDict[prev.Notes[0].CutDirection];

            var expectedParity = prev.Parity == Parity.Forehand ? Parity.Backhand : Parity.Forehand;
            float nextAFN = (expectedParity != Parity.Forehand) ?
                ParityUtils.BackhandDict[currNote.CutDirection] :
                ParityUtils.ForehandDict[currNote.CutDirection];

            float AFNChange = currentAFN - nextAFN;

            bool upsideDown = false;
            switch (prev.Parity)
            {
                case Parity.Backhand when (prev.EndAngle > 0 && currNote.CutDirection == 0) || currNote.CutDirection == 8:
                case Parity.Forehand when (prev.EndAngle > 0 && currNote.CutDirection == 1) || currNote.CutDirection == 8:
                    upsideDown = true;
                    break;
            }

            if (prev.EndAngle == 180)
            {
                float altNextAFN = 180 + nextAFN;
                if (altNextAFN < 0)
                {
                    return true;
                }
                return false;
            }

            if (Mathf.Abs(AFNChange) > ParityUtils.ReboundAngleThreshold && !upsideDown)
            {
                return true;
            }

            return false;
        }

        private float GetAngle(int cutDir, Parity parity)
        {
            var dict = parity == Parity.Forehand ? ParityUtils.ForehandDict : ParityUtils.BackhandDict;
            return dict.TryGetValue(cutDir, out float a) ? a : 0;
        }
    }
}