#nullable enable
using System;
using System.Collections.Generic;
using WarningMarker.Configuration;
using Zenject;

namespace WarningMarker.Parity
{
    public class ParityAnalysisManager : IInitializable
    {
        [Inject] private readonly IReadonlyBeatmapData _beatmapData = null!;

        private readonly HashSet<NoteID> _resetNotes = new HashSet<NoteID>();
        public bool IsReady { get; private set; }

        public void Initialize()
        {
            if (!PluginConfig.ShowResets || !PluginConfig.EnableAdvancedDetection) return;

            AnalyzeMap();
        }

        private void AnalyzeMap()
        {
            var rawNotes = _beatmapData.GetBeatmapDataItems<NoteData>(0);

            var notes = new List<NoteData>(2000);

            foreach (var n in rawNotes)
            {
                if (n.colorType != ColorType.None)
                {
                    notes.Add(n);
                }
            }

            if (notes.Count == 0) return;

            notes.Sort((a, b) => a.time.CompareTo(b.time));

            var generator = new SwingGenerator();
            var leftSwings = generator.GenerateSwings(notes, false);
            var rightSwings = generator.GenerateSwings(notes, true);

            var checker = new ParityCheck();
            checker.Analyze(leftSwings);
            checker.Analyze(rightSwings);

            foreach (var swing in leftSwings)
            {
                if (swing.IsReset)
                {
                    foreach (var n in swing.Notes) _resetNotes.Add(new NoteID(n.OriginalNoteData));
                }
            }

            foreach (var swing in rightSwings)
            {
                if (swing.IsReset)
                {
                    foreach (var n in swing.Notes) _resetNotes.Add(new NoteID(n.OriginalNoteData));
                }
            }

            IsReady = true;
        }

        public bool IsReset(NoteData note)
        {
            return _resetNotes.Contains(new NoteID(note));
        }

        private readonly struct NoteID : IEquatable<NoteID>
        {
            public readonly int Type;
            public readonly float Time;
            public readonly int LineIndex;
            public readonly int NoteLineLayer;
            public readonly int CutDirection;

            public NoteID(NoteData note)
            {
                Type = (int)note.colorType;
                Time = note.time;
                LineIndex = note.lineIndex;
                NoteLineLayer = (int)note.noteLineLayer;
                CutDirection = (int)note.cutDirection;
            }

            public bool Equals(NoteID other)
            {
                return Type == other.Type &&
                       Math.Abs(Time - other.Time) < 0.01f &&
                       LineIndex == other.LineIndex &&
                       NoteLineLayer == other.NoteLineLayer &&
                       CutDirection == other.CutDirection;
            }

            public override bool Equals(object? obj) => obj is NoteID other && Equals(other);

            public override int GetHashCode()
            {
                unchecked
                {
                    var hashCode = Type;
                    hashCode = (hashCode * 397) ^ LineIndex;
                    hashCode = (hashCode * 397) ^ NoteLineLayer;
                    hashCode = (hashCode * 397) ^ CutDirection;
                    return hashCode;
                }
            }
        }
    }
}