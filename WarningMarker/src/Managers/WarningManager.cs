#nullable enable
using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using ModestTree;
using UnityEngine;
using Zenject;
using WarningMarker.Configuration;
using WarningMarker.Utilities;
using WarningMarker.Parity;

namespace WarningMarker.Managers;

internal class WarningManager : IInitializable, IDisposable
{
    [Inject, UsedImplicitly]
    private readonly BeatmapObjectManager _beatmapObjectManager = null!;

    [Inject, UsedImplicitly]
    private readonly WarningMarker.Pool _pool = null!;

    [Inject, UsedImplicitly]
    private readonly ParityAnalysisManager _parityAnalysisManager = null!;

    public void Initialize()
    {
        _beatmapObjectManager.noteWasSpawnedEvent += OnNoteWasSpawned;
        _beatmapObjectManager.noteWasDespawnedEvent += OnNoteWasDespawned;
    }

    public void Dispose()
    {
        _beatmapObjectManager.noteWasSpawnedEvent -= OnNoteWasSpawned;
        _beatmapObjectManager.noteWasDespawnedEvent -= OnNoteWasDespawned;
    }

    private readonly WarningMarker?[] _activeMarkers = new WarningMarker?[2];
    private readonly NoteController?[] _activeNotes = new NoteController?[2];

    private void OnNoteWasSpawned(NoteController noteController)
    {
        if (!ShouldNotify(noteController)) return;

        var colorType = noteController.noteData.colorType;
        if (colorType is not (ColorType.ColorA or ColorType.ColorB)) return;

        int index = (int)colorType;

        if (_activeMarkers[index] != null)
        {
            _pool.Despawn(_activeMarkers[index]!);
            _activeMarkers[index] = null;
            _activeNotes[index] = null;
        }

        var marker = _pool.Spawn(noteController);
        _activeMarkers[index] = marker;
        _activeNotes[index] = noteController;
    }

    private void OnNoteWasDespawned(NoteController noteController)
    {
        var colorType = noteController.noteData.colorType;
        if (colorType is not (ColorType.ColorA or ColorType.ColorB)) return;

        int index = (int)colorType;

        if (_activeNotes[index] == noteController && _activeMarkers[index] != null)
        {
            _pool.Despawn(_activeMarkers[index]!);
            _activeMarkers[index] = null;
            _activeNotes[index] = null;
        }
    }

    private bool ShouldNotify(NoteController noteController)
    {
        var noteData = noteController.noteData;
        if (noteData.colorType == ColorType.None) return false;

        var passesLayerFilter = ShouldShowLayer(noteData.noteLineLayer);

        if (PluginConfig.ShowAllNotes) return passesLayerFilter;

        var isReset = ShouldNotifyReset(noteData);
        var isHorizontal = ShouldNotifyHorizontals(noteData);

        return passesLayerFilter && (isReset || isHorizontal);
    }

    private static bool ShouldShowLayer(NoteLineLayer layer)
    {
        return layer switch
        {
            NoteLineLayer.Top => PluginConfig.ShowTopLayer,
            NoteLineLayer.Upper => PluginConfig.ShowMiddleLayer,
            NoteLineLayer.Base => PluginConfig.ShowBottomLayer,
            _ => true
        };
    }

    private static bool ShouldNotifyHorizontals(NoteData noteData)
    {
        if (!PluginConfig.ShowHorizontals) return false;
        if (noteData.noteLineLayer is not (NoteLineLayer.Base or NoteLineLayer.Top)) return false;
        return noteData.cutDirection is NoteCutDirection.Left or NoteCutDirection.Right;
    }

    private float _lastLeftTime;
    private float _lastRightTime;
    private NoteCutDirection _lastLeftDirection = NoteCutDirection.Up;
    private NoteCutDirection _lastRightDirection = NoteCutDirection.Up;

    private bool ShouldNotifyReset(NoteData noteData)
    {
        if (!PluginConfig.ShowResets) return false;

        bool isReset = false;

        if (PluginConfig.EnableAdvancedDetection)
        {
            isReset = _parityAnalysisManager.IsReset(noteData);
        }

        if (!isReset && PluginConfig.EnableSimpleDetection)
        {
            isReset = noteData.colorType switch
            {
                ColorType.ColorA => CheckAngle(noteData.cutDirection, noteData.time, ref _lastLeftDirection, ref _lastLeftTime),
                ColorType.ColorB => CheckAngle(noteData.cutDirection, noteData.time, ref _lastRightDirection, ref _lastRightTime),
                _ => false
            };
        }
        else if (!PluginConfig.EnableSimpleDetection)
        {
            _ = noteData.colorType switch
            {
                ColorType.ColorA => CheckAngle(noteData.cutDirection, noteData.time, ref _lastLeftDirection, ref _lastLeftTime),
                ColorType.ColorB => CheckAngle(noteData.cutDirection, noteData.time, ref _lastRightDirection, ref _lastRightTime),
                _ => false
            };
        }

        return isReset;
    }

    private static bool CheckAngle(
        NoteCutDirection currentDirection,
        float currentTime,
        ref NoteCutDirection lastDirection,
        ref float lastTime
    )
    {
        var tooLittleTime = Mathf.Abs(currentTime - lastTime) < 0.15;
        lastTime = currentTime;

        if (currentDirection is NoteCutDirection.Any)
        {
            if (!tooLittleTime) lastDirection = lastDirection.GetOppositeCutDirection();
            return false;
        }

        var angle = lastDirection.GetAngle(currentDirection);
        lastDirection = currentDirection;
        return !tooLittleTime && angle < 90.0f;
    }
}