using UnityEngine;
using Zenject;

public class DefaultBoardFactory : BoardFactory
{
    private const string DEFAULT_BACKGROUND = "Board/DefaultBackground";
    private const string DEFAULT_BACKGROUND_BOARD = "Board/DefaultBackgroundBoard";

    public DefaultBoardFactory(IInstantiator instantiator, GridService gridService) : base(instantiator, gridService)
    {
    }

    public override void CreateBackground()
    {
        var gm = _instantiator.InstantiatePrefabResource(DEFAULT_BACKGROUND);
        var halfSize = new Vector2(_gridService.GridModel.Size.x / 2f, _gridService.GridModel.Size.y / 2f);
        var pos = new Vector2(halfSize.y * _gridService.GridModel.gridSize.x, halfSize.x * _gridService.GridModel.gridSize.y);
        gm.transform.position = new Vector3(pos.x, pos.y, gm.transform.position.z);
        gm.transform.localScale = new Vector3(150f, 150f, 30f);
    }

    public override void CreateBackgroundBoard()
    {
        var gm = _instantiator.InstantiatePrefabResource(DEFAULT_BACKGROUND_BOARD);
        var halfSize = new Vector2(_gridService.GridModel.Size.x / 2f, _gridService.GridModel.Size.y / 2f);
        var pos = new Vector2(halfSize.y * _gridService.GridModel.gridSize.x, halfSize.x * _gridService.GridModel.gridSize.y);
        gm.transform.position = new Vector3(pos.x, pos.y, gm.transform.position.z);
        Vector3 scale = new Vector3(_gridService.GridModel.Size.y * _gridService.GridModel.gridSize.x,
            _gridService.GridModel.Size.x * _gridService.GridModel.gridSize.y, 11);
        gm.transform.localScale = scale;
    }
}
