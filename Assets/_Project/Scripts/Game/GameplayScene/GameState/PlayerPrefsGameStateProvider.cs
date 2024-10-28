using R3;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsGameStateProvider : IGameStateProvider
{
    private const string PUZZLE_STATE_KEY = nameof(PUZZLE_STATE_KEY);    
    private const string GRIS_STATE_KEY = nameof (GRIS_STATE_KEY);
    public PuzzleStateProxy PuzzleState { get; private set; }

    public GridStateProxy GridState {  get; private set; }

    private PuzzleState _puzzleStateOrigin;
    private GridState _gridStateOrigin;

    public Observable<PuzzleStateProxy> LoadPuzzleState()
    {
        if (!PlayerPrefs.HasKey(PUZZLE_STATE_KEY))
        {
            PuzzleState = CreateGameStateFromSettings();
            Debug.Log("Game State Created From Settings: " + JsonUtility.ToJson(_puzzleStateOrigin, true));
            
            SavePuzzleState();
        }
        else
        {
            var json = PlayerPrefs.GetString(PUZZLE_STATE_KEY);
            _puzzleStateOrigin = JsonUtility.FromJson<PuzzleState>(json);
            PuzzleState = new PuzzleStateProxy(_puzzleStateOrigin);

            Debug.Log("Game State Loaded: " + json);
        }

        return Observable.Return(PuzzleState);
    }

    public Observable<bool> SavePuzzleState()
    {
        var json = JsonUtility.ToJson(_puzzleStateOrigin, true);
        PlayerPrefs.SetString(PUZZLE_STATE_KEY, json);

        return Observable.Return(true);
    }
    public Observable<bool> ResetPuzzleState()
    {
        PuzzleState = CreateGameStateFromSettings();
        SavePuzzleState();

        return Observable.Return(true);
    }
    private PuzzleStateProxy CreateGameStateFromSettings()
    {
        _puzzleStateOrigin = new PuzzleState
        {
            puzzleEntities = new List<PuzzleEntity>
            {

            }
        };
        return new PuzzleStateProxy(_puzzleStateOrigin);
    }

    public Observable<GridStateProxy> LoadGridState()
    {
        if (!PlayerPrefs.HasKey(GRIS_STATE_KEY))
        {

        }
        else
        {
            var json = PlayerPrefs.GetString(GRIS_STATE_KEY);
            _gridStateOrigin = JsonUtility.FromJson<GridState>(json);
            GridState = new GridStateProxy(_gridStateOrigin);

            Debug.Log("Game State Loaded: " + json);
        }

        return Observable.Return(GridState);
    }


    public Observable<bool> SaveGridState()
    {
        throw new NotImplementedException();
    }

    public Observable<bool> ResetGridState()
    {
        throw new NotImplementedException();
    }
}
