using System.Collections.Generic;
using System.Linq;
using WarningMarker.Parity.Structs;
using UnityEngine;

namespace WarningMarker.Parity
{
    public class SwingGenerator
    {
        public List<ParitySwing> GenerateSwings(List<NoteData> notes, bool rightHand)
        {
            var swings = new List<ParitySwing>(notes.Count / 2);
            var handNotes = new List<NoteData>(notes.Count / 2);

            for (int i = 0; i < notes.Count; i++)
            {
                var n = notes[i];
                if ((rightHand && n.colorType == ColorType.ColorB) || (!rightHand && n.colorType == ColorType.ColorA))
                {
                    handNotes.Add(n);
                }
            }

            handNotes.Sort((a, b) => a.time.CompareTo(b.time));

            if (handNotes.Count == 0) return swings;

            var currentSwing = new ParitySwing
            {
                IsRightHand = rightHand,
                IsReset = false,
                Parity = Parity.Undecided
            };

            currentSwing.AddNote(new ParityNote(handNotes[0]));
            currentSwing.StartBeat = handNotes[0].time;
            currentSwing.StartPos = new Vector2(handNotes[0].lineIndex, (int)handNotes[0].noteLineLayer);

            for (int i = 1; i < handNotes.Count; i++)
            {
                var note = handNotes[i];
                var prevNote = handNotes[i - 1];
                var parityNote = new ParityNote(note);

                bool isStack = Mathf.Abs(note.time - prevNote.time) < ParityUtils.SwingTimeThreshold;
                bool isSlider = Mathf.Abs(note.time - prevNote.time) <= 0.26f && IsSlider(prevNote, note);

                if (isStack || isSlider)
                {
                    currentSwing.AddNote(parityNote);
                }
                else
                {
                    currentSwing.EndBeat = prevNote.time;
                    currentSwing.EndPos = new Vector2(prevNote.lineIndex, (int)prevNote.noteLineLayer);
                    swings.Add(currentSwing);

                    currentSwing = new ParitySwing
                    {
                        IsRightHand = rightHand,
                        IsReset = false,
                        Parity = Parity.Undecided,
                        StartBeat = note.time,
                        StartPos = new Vector2(note.lineIndex, (int)note.noteLineLayer)
                    };
                    currentSwing.AddNote(parityNote);
                }
            }

            if (currentSwing.Notes.Count > 0)
            {
                var lastNote = currentSwing.Notes.Last();
                currentSwing.EndBeat = lastNote.Beat;
                currentSwing.EndPos = new Vector2(lastNote.X, lastNote.Y);
                swings.Add(currentSwing);
            }

            return swings;
        }

        private bool IsSlider(NoteData a, NoteData b)
        {
            return a.cutDirection == b.cutDirection ||
                   a.cutDirection == NoteCutDirection.Any ||
                   b.cutDirection == NoteCutDirection.Any;
        }
    }
}