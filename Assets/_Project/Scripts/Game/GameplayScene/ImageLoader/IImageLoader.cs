using R3;
using UnityEngine;

public interface IImageLoader
{
    Observable<bool> LoadImages(int imageId, int decoratorId, out Texture2D image, out Texture2D decorator);   
}
