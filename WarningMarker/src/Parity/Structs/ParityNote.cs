namespace WarningMarker.Parity.Structs
{
    public readonly struct ParityNote
    {
        public readonly int X;
        public readonly int Y;
        public readonly int CutDirection;
        public readonly int ColorType;
        public readonly float Beat;
        public readonly float Rotation;

        public readonly NoteData OriginalNoteData;

        public ParityNote(NoteData noteData)
        {
            X = noteData.lineIndex;
            Y = (int)noteData.noteLineLayer;
            CutDirection = (int)noteData.cutDirection;
            ColorType = (int)noteData.colorType;
            Beat = noteData.time;
            Rotation = noteData.cutDirectionAngleOffset;
            OriginalNoteData = noteData;
        }
    }
}