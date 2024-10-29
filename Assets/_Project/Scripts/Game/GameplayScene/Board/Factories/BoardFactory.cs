using Zenject;

public abstract class BoardFactory
{
    protected IInstantiator _instantiator;
    protected GridService _gridService;
    public BoardFactory(IInstantiator instantiator, GridService gridService)
    {
        _instantiator = instantiator;
        _gridService = gridService;
    }
    public abstract void CreateBackground();
    public abstract void CreateBackgroundBoard();
}
