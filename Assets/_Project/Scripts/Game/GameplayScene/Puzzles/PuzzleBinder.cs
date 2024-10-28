using UnityEngine;
using R3;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class PuzzleBinder : MonoBehaviour, IControllable
{
    [field: SerializeField] public PuzzleGroupBinder Parent { get; set; }
    public Vector3 Position => transform.localPosition;

    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private float _moveSpeed = 25f;
    private PuzzleViewModel _viewModel;
    private BoxCollider _boxCollider;
    private readonly Vector3 _rotationAxis = new Vector3(0, 0, 1);

    private bool _isRotating;


    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void Bind(PuzzleViewModel viewModel)
    {
        _viewModel = viewModel;
        viewModel.Position.Subscribe(pos =>
        {
            StartCoroutine(MoveToPosition(pos));
        });
        viewModel.Rotation.Skip(1).Subscribe(rotation =>
        {
            StartCoroutine(RotateLerp());
        });

    }
    public IEnumerator MoveToPosition(Vector3 position, float time = 0.1f)
    {
        float ellapseTime = 0;
        while (ellapseTime < time)
        {
            transform.position = Vector3.Lerp(transform.position, position, ellapseTime / time);
            ellapseTime += Time.deltaTime;
            yield return null;
        }
        transform.position = position;
    }
    public void Move(Vector3 position)
    {
        transform.position = Vector3.MoveTowards(transform.position, position, _moveSpeed * Time.deltaTime);
    }
    public void Rotate()         
    {
        if (_isRotating)
            return;
        _viewModel.Rotate();
    }
    public IEnumerator RotateLerp(float time = 0.2f)
    {
        _isRotating = true;
        float elaspedTime = 0.0f;
        float prevTrans = transform.rotation.eulerAngles.z;
        while (elaspedTime < time)
        {
            elaspedTime += Time.deltaTime;
            transform.rotation = Quaternion.Euler(0, 0, prevTrans - Mathf.Lerp(0, 90, elaspedTime / time));
            yield return null;
        }
        transform.rotation = Quaternion.Euler(0, 0, prevTrans - 90f);
        _isRotating = false;
    }
    public void UpdateTexture(PuzzleElementToGenerate puzzleElement)
    {
        _spriteRenderer.sprite = Sprite.Create(puzzleElement.texture, 
            new Rect(0, 0, puzzleElement.texture.width, puzzleElement.texture.height), 
            puzzleElement.pivot, 100);
        _boxCollider = gameObject.AddComponent<BoxCollider>();
        _boxCollider.isTrigger = true;
    }
    public void Drop()
    {
        _viewModel.MovePuzzleToPosition(transform.position);
        _viewModel.Drop();
    }

}
