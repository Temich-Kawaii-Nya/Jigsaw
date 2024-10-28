using R3;
using UnityEngine;

public class DefaultImageLoader : IImageLoader
{
    public Observable<bool> LoadImages(int imageId, int decoratorId, out Texture2D image, out Texture2D decorator)
    {
        image = Resources.Load<Texture2D>("Images/" + imageId);
        decorator = Resources.Load<Texture2D>("Decorators/" + decoratorId);
        return Observable.Return(true);
    }
}
