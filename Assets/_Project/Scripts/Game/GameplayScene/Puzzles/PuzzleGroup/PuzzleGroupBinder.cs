using R3;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleGroupBinder : MonoBehaviour, IControllable
{
    public Vector3 Position => transform.position;
    public PuzzleGroupBinder Parent { get; set; }
    private float _moveSpeed = 25f;
    private PuzzleGroupViewModel _viewModel;
    private List<PuzzleBinder> _puzzles = new List<PuzzleBinder>();
    private Vector3 _lastPosition;
    private bool _isRotating;
    private bool _isMoving;

    public void AddPuzzle(PuzzleBinder binder)
    {
        binder.Parent = this;

        _puzzles.Add(binder);
    }
    public void RemovePuzzle(PuzzleBinder binder) 
    { 
        binder.Parent = null;
        _puzzles.Remove(binder); 
    }

    public void Bind (PuzzleGroupViewModel viewModel)
    {
        _viewModel = viewModel;
        viewModel.Rotation.Skip(1).Subscribe(e =>
        {
            StartCoroutine(RotateLerp());
        });
        _viewModel.Position.Subscribe(e =>
        {
            if (e != transform.position)
                transform.position = e;
        });
    }
    public void Drop()
    {
        _viewModel.Move(transform.position);
        _viewModel.Drop();
    }
    public IEnumerator MoveToPosition(Vector3 position, float time = 0.2f)
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
    public void Move(Vector3 pos)
    {
        if (_isRotating)
            return;
        transform.position = Vector3.MoveTowards(transform.position, pos, _moveSpeed * Time.deltaTime);
    }
    public IEnumerator RotateLerp(float time = 0.1f)
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
        _viewModel.DoAfterRotation();
        _isRotating = false;
    }
    public void Rotate()
    {
        if (_isRotating || _isMoving)
        {
            return;
        }
        _viewModel.Rotate();
    }
}
