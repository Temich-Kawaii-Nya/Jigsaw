using UnityEngine;

public class DefaultPuzzleGenerator : IPuzzleGenerator
{
    public PuzzleElementToGenerate[] PuzzlesToGenerate => _puzzleGrid;
    
    private int _cols = 2;
    private int _rows = 2;

    private int puzzleDefaultSize = 100;

    private PuzzleElementToGenerate[] _puzzleGrid;

    private int _imageId;
    private int _decoratorId;
    private Texture2D _image;
    private Texture2D _decorator;

    public DefaultPuzzleGenerator(int imageId, int decoratorId, Vector2 size)
    {
        _imageId = imageId;
        _decoratorId = decoratorId;
        _cols = (int)size.y;
        _rows = (int)size.x;
    }

    public void Generate()
    {
        GetTextureFromId();
        GeneratePuzzlesFromTexture();
    }

    private void GetTextureFromId() // TODO
    {
        _image = Resources.Load<Texture2D>("Images/" + _imageId);
        _decorator = Resources.Load<Texture2D>("Decorators/" + _decoratorId);
    }

    private void GeneratePuzzlesFromTexture()
    {
        _puzzleGrid = new PuzzleElementToGenerate[_cols * _rows];
        puzzleDefaultSize = Mathf.Clamp(puzzleDefaultSize, _decorator.width * 2, _decorator.width * 4);
        GeneratePuzzlePiece(_image, _decorator, _cols, _rows);
    }
    private void GeneratePuzzlePiece(Texture2D image, Texture2D decorator, int cols, int rows)
    {
        int top;
        int bottom;
        int left;
        int right;

        Vector2 pieceSize = new Vector2(image.width * 1.0f / cols * 1.0f / puzzleDefaultSize, image.height / rows * 1.0f / puzzleDefaultSize);//TODO

        Color[] puzzleDecoratorPixels = decorator.GetPixels();
        Color[] topPixels = puzzleDecoratorPixels;
        Color[] leftPixels = Rotate90(puzzleDecoratorPixels, decorator.width, decorator.height, false);

        for (int y = 0; y < rows; y++) 
        {
            for (int x = 0; x < cols; x++)
            {
                top = y > 0 ? -_puzzleGrid[((y - 1) * _cols + x)].bottom : 0;
                left = x > 0 ? -_puzzleGrid[(y * _cols + x - 1)].right : 0;
                bottom = y < (_rows - 1) ? Random.Range(-1, 1) * 2 + 1 : 0;
                right = x < (_cols - 1) ? Random.Range(-1, 1) * 2 + 1 : 0;


                _puzzleGrid[y * _cols + x] = new PuzzleElementToGenerate
                (
                    top, left, bottom, right,
                    puzzleDefaultSize,
                    decorator,
                    topPixels, leftPixels
                );

                _puzzleGrid[y * _cols + x].texture = ExtractFromImage(_image, _puzzleGrid[y * _cols + x], x, y, puzzleDefaultSize, pieceSize);

                float k = 0.5f;
                if (!Mathf.Abs(_puzzleGrid[y * _cols + x].pixelOffset.height * 1.0f - _puzzleGrid[y * _cols + x].pixelOffset.y * 1.0f).Equals(0f))
                {
                    k = 1f;
                }
                
                _puzzleGrid[y * _cols + x].pivot = new
                //(
                //    (_puzzleGrid[y * _cols + x].texture.width * pieceSize.x) / ((_puzzleGrid[y * _cols + x].pixelOffset.width * 1.0f - _puzzleGrid[y * _cols + x].pixelOffset.x * 1.0f) + _puzzleGrid[y * _cols + x].texture.width * pieceSize.x) * 0.5f,
                //    0.5f - ((l/2 + delta) / delta)
                //);
                (
                    (_puzzleGrid[y * _cols + x].pixelOffset.x * 1.0f / _puzzleGrid[y * _cols + x].texture.width * pieceSize.x),
                    (1f - _puzzleGrid[y * _cols + x].pixelOffset.y * 1.0f / _puzzleGrid[y * _cols + x].texture.height * pieceSize.y)
                );
                float lx = _puzzleGrid[y * _cols + x].texture.width - (_puzzleGrid[y * _cols + x].pixelOffset.x * pieceSize.x + _puzzleGrid[y * _cols + x].pixelOffset.width * pieceSize.x);
                float Lx = _puzzleGrid[y * _cols + x].texture.width;
                float ly = _puzzleGrid[y * _cols + x].texture.height - (_puzzleGrid[y * _cols + x].pixelOffset.y * pieceSize.y + _puzzleGrid[y * _cols + x].pixelOffset.height * pieceSize.y); ;
                float Ly = _puzzleGrid[y * _cols + x].texture.height;

                Debug.Log($"{x} {y}  {ly}   {Ly}");
                _puzzleGrid[y * _cols + x].pivot = new(_puzzleGrid[y * _cols + x].pivot.x + (lx / (Lx * 2f)) , _puzzleGrid[y * _cols + x].pivot.y -  (ly / (Ly * 2f)));

            }
        }

    }

    private Texture2D ExtractFromImage(Texture2D _image, PuzzleElementToGenerate _puzzlElement, int _x, int _y, int _elementBaseSize, Vector2 _elementSizeRatio)
    {
        Color[] pixels = _image.GetPixels
        (
            (int)((_x * _elementBaseSize - _puzzlElement.pixelOffset.x) * _elementSizeRatio.x),
            (int)(_image.height - (_y + 1) * _elementBaseSize * _elementSizeRatio.y - _puzzlElement.pixelOffset.height * _elementSizeRatio.y),
            (int)(_puzzlElement.maskWidth * _elementSizeRatio.x),
            (int)(_puzzlElement.maskHeight * _elementSizeRatio.y)
        );

        Texture2D result = new Texture2D
        (
            (int)(_puzzlElement.maskWidth * _elementSizeRatio.x),
            (int)(_puzzlElement.maskHeight * _elementSizeRatio.y)
        );

        result.wrapMode = TextureWrapMode.Clamp;
        _puzzlElement.ApplyMask(pixels, ref result);

        return result;
    }
    private static Color[] Rotate90(Color[] _source, int _width, int _height, bool _clockwise = true)
    {
        Color[] result = new Color[_source.Length];
        int rotatedPixelId, originalPixelId;

        if (_clockwise)
        {
            for (int y = 0; y < _height; ++y)
            {
                for (int x = 0; x < _width; ++x)
                {
                    rotatedPixelId = (x + 1) * _height - y - 1;
                    originalPixelId = _source.Length - 1 - (y * _width + x);
                    result[rotatedPixelId] = _source[originalPixelId];
                }
            }
        }
        else
        {
            for (int y = 0; y < _height; ++y)
            {
                for (int x = 0; x < _width; ++x)
                {
                    rotatedPixelId = (x + 1) * _height - y - 1;
                    originalPixelId = y * _width + x;
                    result[rotatedPixelId] = _source[originalPixelId];
                }
            }
        }

        return result;
    }

}
