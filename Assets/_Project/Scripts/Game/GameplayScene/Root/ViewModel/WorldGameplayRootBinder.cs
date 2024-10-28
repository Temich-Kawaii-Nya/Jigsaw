using ObservableCollections;
using R3;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class WorldGameplayRootBinder : MonoBehaviour
{
    [SerializeField] private PuzzleBinder _prefub;
    [SerializeField] private PuzzleGroupBinder _groupPrefub;
    [SerializeField] private Texture2D _image;
    [SerializeField] private Texture2D _decorator;

    private readonly Dictionary<int, PuzzleBinder> _createdPuzzlesBindersMap = new();
    private readonly CompositeDisposable _disposables = new();

    private IPuzzleGenerator _generator;
    public void Bind(WorldGameplayRootViewModel viewModel, IPuzzleGenerator generator)
    {
        _generator = generator;

        foreach (var puzzleVM in viewModel.AllPuzzles)
        {
            CreatePuzzle(puzzleVM);
        }
        _disposables.Add(viewModel.AllPuzzles.ObserveAdd().Subscribe(e =>
        {
            CreatePuzzle(e.Value);
        }));
        _disposables.Add(viewModel.AllPuzzles.ObserveRemove().Subscribe(e =>
        {
            DestroyPuzzle(e.Value);
        }));
        _disposables.Add(viewModel.AllGroups.ObserveAdd().Subscribe(e =>
        {
            CreatePuzzleGroup(e.Value);
        }));
        _disposables.Add(viewModel.AllGroups.ObserveRemove().Subscribe(e =>
        {
            DestroyPuzzleGroup(e.Value);
        }));
    }
    public void CreatePuzzle(PuzzleViewModel model)
    {
        var createdPuzzle = Instantiate(_prefub);
        createdPuzzle.Bind(model);
        Debug.Log(_generator.PuzzlesToGenerate.Length);
        Debug.Log(model.PuzzleEntityId);
        createdPuzzle.UpdateTexture(_generator.PuzzlesToGenerate[model.PuzzleEntityId]);
        createdPuzzle.name = $"puzzle {model.PuzzleEntityId}";
        _createdPuzzlesBindersMap[model.PuzzleEntityId] = createdPuzzle;
    }
    public void CreatePuzzleGroup(PuzzleGroupViewModel model)
    {
        Debug.Log("GroupView");
        var createdGroup = Instantiate(_groupPrefub);
        createdGroup.Bind(model);
        model._puzzles.ObserveAdd().Subscribe(e =>
        {
            _createdPuzzlesBindersMap.TryGetValue(e.Value.Id, out PuzzleBinder binder);
            createdGroup.AddPuzzle(binder);
            binder.transform.parent = createdGroup.transform;
        });
        
    }
    public void DestroyPuzzleGroup(PuzzleGroupViewModel model)
    {
       //TODo
    }
    public void DestroyPuzzle(PuzzleViewModel model)
    {
        if (_createdPuzzlesBindersMap.TryGetValue(model.PuzzleEntityId, out PuzzleBinder binder))
        {
            Destroy(binder.gameObject);
            _createdPuzzlesBindersMap.Remove(model.PuzzleEntityId);
        }
    }
    private void OnDestroy()
    {
        _disposables.Dispose();
    }
}
