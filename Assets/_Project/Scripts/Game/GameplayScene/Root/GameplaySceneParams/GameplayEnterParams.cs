using UnityEngine;

public class GameplayEnterParams
{
    public int ImageId { get; }
    public Vector2Int Size { get; }
    public bool Rotate { get; }
    public int DecoratorId { get; }
    public GameplayEnterParams(int imageId, int decoratorId, Vector2Int size, bool rotate)
    {
        ImageId = imageId;
        DecoratorId = decoratorId;
        Size = size;
        Rotate = rotate;
    }
}
