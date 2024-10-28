using ObservableCollections;
using System.Collections.Generic;
using UnityEngine;

public class BoardService 
{
    private readonly GridService _gridService;
    private readonly GridProxy _gridProxy;

    public BoardService(GridService gridService)
    {
        _gridService = gridService;
    }

    public void ProxyInRightPlace(PuzzleEntityProxy proxy)
    {
    }
}
