using UnityEngine;

public class PuzzleElementToGenerate
{
    public int top;
    public int left;
    public int bottom;
    public int right;


    public Texture2D texture;
    public Vector2 pivot;


    public Rect pixelOffset;

    float[] mask;
    public int maskWidth;
    public int maskHeight;
    public int maskHeigsht;
    public PuzzleElementToGenerate(int _top, int _left, int _bottom, int _right, int _baseSize, Texture2D _subElement, Color[] _topPixels, Color[] _leftPixels)
    {
        top = _top;
        left = _left;
        bottom = _bottom;
        right = _right;


        maskWidth = _baseSize;
        maskHeight = _baseSize;

        if (top == 1)
        {
            maskHeight += _subElement.height;
            pixelOffset.y = _subElement.height;
        }

        if (bottom == 1)
        {
            maskHeight += _subElement.height;
            pixelOffset.height = _subElement.height;
        }


        if (left == 1)
        {
            maskWidth += _subElement.height;
            pixelOffset.x = _subElement.height;
        }

        if (right == 1)
        {
            maskWidth += _subElement.height;
            pixelOffset.width = _subElement.height;
        }

        mask = new float[maskWidth * maskHeight];
        for (int y = maskHeight - 1 - (int)pixelOffset.y; y > maskHeight - 1 - (int)pixelOffset.y - _baseSize; y--)
            for (int x = (int)pixelOffset.x; x < (int)pixelOffset.x + _baseSize; x++)
                mask[y * maskWidth + x] = 1.0f;
																																																																											
        if (top != 0)
            FillMask(
                        (int)pixelOffset.x + (_baseSize - _subElement.width) / 2,
                        maskHeight - _subElement.height,
                        (int)pixelOffset.x + (_baseSize - _subElement.width) / 2 + _subElement.width,
                        maskHeight,
                        maskWidth,
                        ref mask,
                        _topPixels,
                        top,
                        top
                    );

        if (bottom != 0)
            FillMask(
                        (int)pixelOffset.x + (_baseSize - _subElement.width) / 2,
                        0,
                        (int)pixelOffset.x + (_baseSize - _subElement.width) / 2 + _subElement.width,
                        _subElement.height,
                        maskWidth,
                        ref mask,
                        _topPixels,
                        -bottom,
                        bottom
                    );
        
        if (left != 0)
            FillMask(
                        0,
                        maskHeight - (int)pixelOffset.y - _baseSize + (_baseSize - _subElement.width) / 2,
                        _subElement.height,
                        maskHeight - (int)pixelOffset.y - _baseSize + (_baseSize - _subElement.width) / 2 + _subElement.width,
                        maskWidth,
                        ref mask,
                        _leftPixels,
                        left,
                        left
                    );
										
        if (right != 0)
            FillMask(
                        maskWidth - _subElement.height,
                        maskHeight - (int)pixelOffset.y - _baseSize + (_baseSize - _subElement.width) / 2,
                        maskWidth,
                        maskHeight - (int)pixelOffset.y - _baseSize + (_baseSize - _subElement.width) / 2 + _subElement.width,
                        maskWidth,
                        ref mask,
                        _leftPixels,
                        -right,
                        right
                    );

    }
    public void ApplyMask(Color[] _sourcePixels, ref Texture2D _result)
    {
        int width = _result.width;
        int height = _result.height;

        for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
                _sourcePixels[y * width + x].a *= mask[(y * maskHeight / height) * maskWidth + (x * maskWidth / width)];


        _result.SetPixels(_sourcePixels);
        _result.Apply();

    }
    void FillMask(int _x, int _y, int _width, int _height, int _rowWidth, ref float[] _mask, Color[] _fill = null, int invertion = 1, int _negative = 0)
    {
        int fillPixelNum = invertion < 0 ? _fill.Length - 1 : 0;

        if (_negative < 0)
            for (int y = _y; y < _height; y++)
                for (int x = _x; x < _width; x++)
                {
                    _mask[y * _rowWidth + x] *= 1 - _fill[fillPixelNum].a;
                    fillPixelNum += invertion;
                }
        else
            for (int y = _y; y < _height; y++)
                for (int x = _x; x < _width; x++)
                {
                    _mask[y * _rowWidth + x] = _fill[fillPixelNum].a;
                    fillPixelNum += invertion;
                }

    }
}
