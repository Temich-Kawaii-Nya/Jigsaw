using UnityEngine;
using Zenject;

public class GameplayEnterParamsAdapter
{
    public Texture2D Image { get; private set; }
    public Texture2D Decorator { get; private set; }   
    public Vector2 CellSize { get; private set; }
    public Vector2Int GridSize { get; private set; }
    public bool Rotate { get; private set; }

    private int puzzleDefaultSize = 100;
    
    [Inject]
    private IImageLoader _imageLoader;


    public GameplayEnterParamsAdapter(GameplayEnterParams enterParams, IImageLoader imageLoader)
    {
        _imageLoader = imageLoader;
        GridSize = enterParams.Size;
        Rotate = enterParams.Rotate;
        _imageLoader.LoadImages(enterParams.ImageId, enterParams.DecoratorId, out Texture2D Image, out Texture2D Decorator);
        this.Image = Image;
        this.Decorator = Decorator;
        CellSize = CalculateCellSize(this.Image, this.Decorator, GridSize);
    }
    private Vector2 CalculateCellSize(Texture2D image, Texture decorator, Vector2 gridSize)
    {
        //puzzleDefaultSize = Mathf.Clamp(puzzleDefaultSize, decorator.width * 2, decorator.width * 4);
        Debug.Log(puzzleDefaultSize + " 2");
        Vector2 cellSize = new Vector2(image.width / gridSize.y * 1.0f / puzzleDefaultSize, image.height / gridSize.x * 1.0f / puzzleDefaultSize);
        return cellSize;
    }
}
