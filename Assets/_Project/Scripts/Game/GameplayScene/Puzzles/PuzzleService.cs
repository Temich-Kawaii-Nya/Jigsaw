using System.Collections.Generic;
using ObservableCollections;
using UnityEngine;
using R3;
public class PuzzleService
{
    public IObservableCollection<PuzzleViewModel> AllPuzzles => _allPuzzles;
    public IObservableCollection<PuzzleGroupViewModel> AllGroups => _allGrups;

    private readonly ObservableList<PuzzleViewModel> _allPuzzles = new();
    private readonly ObservableList<PuzzleGroupViewModel> _allGrups = new();

    private readonly Dictionary<int, PuzzleViewModel> _puzzlesMap = new();
    private readonly Dictionary<int, PuzzleEntityProxy> _puzzlesProxiesMap = new();

    private readonly IGameStateProvider _gameStateProvider;
    private readonly GridService _gridService;
    private readonly BoardService _boardService;

    public PuzzleService(
        IObservableCollection<PuzzleEntityProxy> puzzlesProxies, 
        IGameStateProvider gameStateProvider, 
        GridService gridService,
        BoardService boardService)
    {
        _gameStateProvider = gameStateProvider;
        _gridService = gridService;
        _boardService = boardService;

        foreach (var puzzle in puzzlesProxies)
        {
            CreatePuzzleViewModel(puzzle);
        }
        puzzlesProxies.ObserveAdd().Subscribe(e =>
        {
            CreatePuzzleViewModel(e.Value);
        });
        puzzlesProxies.ObserveRemove().Subscribe(e =>
        {
            RemovePuzzleViewModel(e.Value);
        });

    }
    public PuzzleGroupViewModel CreateGroup()
    {
        var model = new PuzzleGroupEntity(_gameStateProvider.PuzzleState.GetPuzzleId(), Vector3.zero);
        var proxy = new PuzzleGroupEntityProxy(model);
        var grup = new PuzzleGroupViewModel(proxy, this);
        _allGrups.Add(grup);
        return grup;
    }
    public PuzzleGroupViewModel CreateGroupInPos(Vector3 pos, int rotation = 0)
    {
        var model = new PuzzleGroupEntity(_gameStateProvider.PuzzleState.GetPuzzleId(), pos);
        model.rotation = rotation;
        var proxy = new PuzzleGroupEntityProxy(model);
        var grup = new PuzzleGroupViewModel(proxy, this);
        _allGrups.Add(grup);
        return grup;
    }

    public PuzzleEntityProxy CreatePuzzle()
    {
        var puzzleId = _gameStateProvider.PuzzleState.GetPuzzleId();

        var newPuzzle = new PuzzleEntity
        {
            id = puzzleId,
            position = Vector3.zero,
            Rotation = 0
        };

        var newProxy = new PuzzleEntityProxy(newPuzzle);    

        _gameStateProvider.PuzzleState.Puzzles.Add(newProxy);
        _puzzlesProxiesMap.Add(puzzleId, newProxy);
        return newProxy;
    }
    public PuzzleEntityProxy CreatePuzzleWithIndex(Vector2Int index)
    {
        var puzzleId = _gameStateProvider.PuzzleState.GetPuzzleId();

        var newPuzzle = new PuzzleEntity
        {
            id = puzzleId,
            position = Vector3.zero,
            Rotation = 0,
            index = index
        };


        var newProxy = new PuzzleEntityProxy(newPuzzle);

        _gameStateProvider.PuzzleState.Puzzles.Add(newProxy);
        _puzzlesProxiesMap.Add(puzzleId, newProxy);
        return newProxy;
    }
    public void RotatePuzzle(int puzzleEntityId)
    {
        _puzzlesProxiesMap.TryGetValue(puzzleEntityId, out PuzzleEntityProxy proxy);
        proxy.Rotation.Value = (proxy.Rotation.Value - 90) % 360;
    }
    public void RotatePuzzleGroup(PuzzleGroupEntityProxy proxy)
    {
        proxy.Rotation.Value = (proxy.Rotation.Value - 90) % 360;
        //DoAfterRotatePuzzlesGroup(proxy);û
    }
    public void DoAfterRotatePuzzlesGroup(PuzzleGroupEntityProxy proxy)
    {
        for (int i = 0; i < proxy._puzzles.Count; i++)
        {
            var puzzle = proxy._puzzles[i];
            var currentPosition = puzzle.Position.Value;

            var newPos = myRotateAround(currentPosition, proxy.Position.Value, new Vector3(0, 0, 1), -90);

            puzzle.Position.Value = newPos;
        }
        
    }
    public Vector3 myRotateAround(Vector3 start, Vector3 point, Vector3 axis, float angle)
    {
        Vector3 vector = start;
        Quaternion quaternion = Quaternion.AngleAxis(angle, axis);
        Vector3 vector2 = vector - point;
        vector2 = quaternion * vector2;
        vector = point + vector2;
        start = vector;
        return start;
    }
    public void MovePuzzleToPosition(int puzzleEntityId, Vector3 newPosition)
    {
        _puzzlesProxiesMap.TryGetValue(puzzleEntityId, out PuzzleEntityProxy proxy);
        proxy.Position.Value = newPosition;
    }
    public void MoveGroupToPosition(PuzzleGroupEntityProxy proxy, Vector3 newPos)
    {
        var index = _gridService.GetCellFromWorldPosition(newPos);
        newPos = _gridService.GetWorldPositionFromIndex((int)index.x, (int)index.y);
        var prevPos = proxy.Position.Value;
        var delta = newPos - prevPos;
        delta.z = 0;
        for (int i = 0; i < proxy._puzzles.Count; i++)
        {
            proxy._puzzles[i].Position.Value += delta;
        }
        var delta2 = newPos - prevPos;
        proxy.Position.Value = proxy.Position.Value + delta;
        proxy.Position.ForceNotify();
    }
    public void CheckPuzzlePosition(int puzzleEntityId)
    {
        _puzzlesProxiesMap.TryGetValue(puzzleEntityId, out PuzzleEntityProxy proxy);
        var index = _gridService.GetCellFromWorldPosition(proxy.Position.Value);
        if (proxy.Parent.Value == null)
            MovePuzzleToPosition(puzzleEntityId, _gridService.GetWorldPositionFromIndex((int)index.x, (int)index.y));
        if (new Vector2Int((int)index.x, (int)index.y) != proxy.Index)
            return;
        if (proxy.Rotation.Value != 0)
            return;
        _gridService.InserPuzzleAtCell(proxy, (int)index.y, (int)index.x);
    }
    public void CheckCombinations(PuzzleEntityProxy proxy)
    {
        Debug.Log("CHeck");
        var list = GetProxiesNearby(proxy);
        for (int i = 0; i < list.Count; i++)
        {
            TryCombine(proxy.Id, list[i].Id);
        }
    }
    public List<PuzzleEntityProxy> GetProxiesNearby(PuzzleEntityProxy proxy)
    {
        var list = new List<PuzzleEntityProxy>();
        foreach (var item in _puzzlesProxiesMap)
        {
            var proxy2 = item.Value;
            if (HasSameParent(proxy, proxy2))
                continue;
            if (!IsPuzzlesNeighbours(proxy.Index, proxy2.Index))
                continue;
            if (!PuzzleInCircle(proxy.Position.Value, proxy2.Position.Value, _gridService.GridModel.gridSize.x * 1.1f))
                continue;
            list.Add(proxy2);
        }
        return list;
    }
    public bool PuzzleInCircle(Vector3 center, Vector3 point, float radius)
    {
        var res = (point.x - center.x) * (point.x - center.x) + (point.y - center.y) + (point.y - center.y);
        var R = radius * radius;

        return res <= R;
    }

    public void TryCombine(int puzzleEntityId1, int puzzleEntityId2)
    {
        _puzzlesProxiesMap.TryGetValue(puzzleEntityId1, out PuzzleEntityProxy proxy1);
        _puzzlesProxiesMap.TryGetValue(puzzleEntityId2, out PuzzleEntityProxy proxy2);
        Debug.Log("0");
        if (HasSameParent(proxy1, proxy2))
            return;
        Debug.Log("1");
        if (!IsPuzzlesNeighbours(proxy1.Index, proxy2.Index))
            return;
        Debug.Log("2");
        if (!HasSameRotation(proxy1, proxy2))
            return;
        Debug.Log("3");
        if (!FromTheRightSide(proxy2, proxy1))
            return;
        Debug.Log("4");
        
        var sub = proxy1.Index - proxy2.Index;
        Vector2 gridSize = new(_gridService.GridModel.gridSize.x, _gridService.GridModel.gridSize.y);

        var rotation = proxy1.Parent.Value == null ? 0 : proxy1.Parent.Value.Rotation.Value;
        switch (proxy1.Rotation.Value + rotation)
        {
            case (0):
                break;
            case (-90):
                sub = new(sub.y, -sub.x);
                gridSize = new(gridSize.y, gridSize.x);
                break;
            case (-180):
                sub = new(-sub.x, -sub.y);
                break;
            case (-270):
                sub = new(-sub.y, sub.x);
                gridSize = new(gridSize.y, gridSize.x);
                break;
        }
        
        proxy1.IsCombined = true;
        proxy2.IsCombined = true;

        Vector3 posDelta = gridSize * sub;

        MovePuzzleToPosition(puzzleEntityId1, proxy2.Position.Value + posDelta);

        Debug.Log("5");
        var list = new List<PuzzleEntityProxy>();
        if (proxy1.Parent.Value != null)
            list.AddRange(proxy1.Parent.Value._puzzles);
        else
            list.Add(proxy1);
        if (proxy2.Parent.Value != null)
            list.AddRange(proxy2.Parent.Value._puzzles);
        else
            list.Add(proxy2);
        
        var group = CreateGroupInPos(proxy1.Position.Value, proxy1.Rotation.Value);

        

        Debug.Log("List count" + list.Count);
        for (int i = 0; i < list.Count; i++)
        {
            group.AddPuzzleToGroup(list[i]);
        }    
    }
    private bool FromTheRightSide(PuzzleEntityProxy proxy1, PuzzleEntityProxy proxy2)
    {
        var sub = proxy1.Index - proxy2.Index;
        var currentPos = GetPositionNormolized(proxy1, proxy2);



        var rotation = proxy1.Parent.Value == null ? 0 : proxy1.Parent.Value.Rotation.Value;
        return CheckPosition(currentPos, sub, proxy1.Rotation.Value + rotation); //TODO 
    }
    private bool CheckPosition(Vector2 current, Vector2 correct, int rotation)
    {
        switch (rotation)
        {
            case 0:
                return current == correct;
            case -90:
                correct = new(correct.y, -correct.x);
                return current == correct;
            case -180:
                correct = new(-correct.x, -correct.y);
                return current == correct;
            case -270:
                correct = new(-correct.y, correct.x);
                return current == correct;
            default: return false;
        }
    }
    private Vector2 GetPositionNormolized(PuzzleEntityProxy proxy1, PuzzleEntityProxy proxy2)
    {
        Vector2 res = Vector2.zero;

        var xRes = proxy1.Position.Value.x - proxy2.Position.Value.x;
        var yRes = proxy1.Position.Value.y - proxy2.Position.Value.y;

        if (xRes > 0.0f)
            res.x = 1;
        if (xRes < 0.0f)
            res.x = -1;
        if (yRes > 0.0f)
            res.y = 1;
        if (yRes < 0.0f)
            res.y = -1;

        if (Mathf.Approximately(proxy1.Position.Value.y, proxy2.Position.Value.y))
        {
            res.y = 0;
        }
        if (Mathf.Approximately(proxy1.Position.Value.x, proxy2.Position.Value.x))
        {
            res.x = 0;
        }

        return res;

    }
    private bool HasSameParent(PuzzleEntityProxy proxy1, PuzzleEntityProxy proxy2)
    {
        if (proxy1.Parent.Value == null)  
            return false;
        if (proxy2.Parent.Value == null)
            return false;
        if (proxy1.Parent.Value != proxy2.Parent.Value)
            return false;

        return true;
    }
    private bool HasSameRotation(PuzzleEntityProxy proxy1, PuzzleEntityProxy proxy2)
    {
        int rotation1 = proxy1.Rotation.Value;
        int rotation2 = proxy2.Rotation.Value;
        if (proxy1.Parent.Value != null)
            rotation1 += proxy1.Parent.Value.Rotation.Value;
        if (proxy2.Parent.Value != null)
            rotation2 += proxy2.Parent.Value.Rotation.Value;

        return rotation1 == rotation2;
    }
    private bool IsPuzzlesNeighbours(Vector2Int index1, Vector2Int index2)
    {
        var sub = index1 - index2;
        if (sub.x == 0 && sub.y == 0)
            return false;
        if (sub.x < -1 || sub.x > 1 || sub.y > 1 || sub.y < -1)
            return false;
        if (sub.x != 0 && sub.y != 0)
            return false;
        return true;
    }

    private void CreatePuzzleViewModel(PuzzleEntityProxy puzzleProxy)
    {
        var puzzleViewModel = new PuzzleViewModel(puzzleProxy, this);
        _allPuzzles.Add(puzzleViewModel);
        _puzzlesMap[puzzleProxy.Id] = puzzleViewModel;
     }
    private void RemovePuzzleViewModel(PuzzleEntityProxy puzzleProxy)
    {
        if (_puzzlesMap.TryGetValue(puzzleProxy.Id, out var puzzleViewModel))
        {
            _allPuzzles.Remove(puzzleViewModel);
            _puzzlesMap.Remove(puzzleProxy.Id);
        }
    }
}
