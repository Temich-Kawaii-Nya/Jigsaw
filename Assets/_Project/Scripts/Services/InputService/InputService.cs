using System;
using System.Collections;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputService : MonoBehaviour, IDisposable
{
    public PlayerInputActions PlayerInputActions { get; private set; }
    public PlayerInputActions.InputActionsActions InputActions { get; private set; }

    public Vector2 MouseDelta => InputActions.MouseDelta.ReadValue<Vector2>();
    public Vector2 MousePos => InputActions.MousePos.ReadValue<Vector2>();

    private PuzzleBinder _binder;
    private bool _isDragging;
    private Vector2 _diffCords;

    private bool _isMoving;

    private void Awake()
    {
        PlayerInputActions = new PlayerInputActions();
        InputActions = PlayerInputActions.InputActions;
        InputActions.Enable();
        AddSub();
    }
    public IControllable CastRayFromTapPostion()
    {
        Vector2 pointerPosition = MousePos;
        Ray ray = Camera.main.ScreenPointToRay(pointerPosition);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.TryGetComponent(out IControllable binder))
            {
                if (binder.Parent != null)
                    return binder.Parent;
                return binder;
            }
        }
        return null;
    }
    public void Move(InputAction.CallbackContext context)
    {
        var binder = CastRayFromTapPostion();
        if (binder == null) return;
        Vector3 hitCords = Camera.main.ScreenToWorldPoint(new Vector3(MousePos.x, MousePos.y, 0));
        Vector3 objCords = new(binder.Position.x, binder.Position.y, 0);
        _diffCords = hitCords - objCords;
        StartCoroutine(Drag(binder));
    }
    public void Rotate(InputAction.CallbackContext context)
    {
        if (_isMoving)
            return;
        var binder = CastRayFromTapPostion();
        if (binder == null) return;
        binder.Rotate();

    }
    private void AddSub()
    {
        InputActions.Tap.performed += Rotate;
        InputActions.Drag.performed += Move;
        InputActions.Drag.canceled += _ => SetDraggingFalse();
    }

    private void RemoveSub()
    {
        InputActions.Tap.performed -= Rotate;
        InputActions.Drag.performed -= Move;
        InputActions.Drag.canceled -= _ => SetDraggingFalse();

    }
    public void Dispose()
    {
        RemoveSub();
        InputActions.Disable();
    }
    private IEnumerator Drag(IControllable view)
    {
        _isDragging = true;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(MousePos);
        while (_isDragging) 
        {
            Vector3 newVector = Camera.main.ScreenToWorldPoint(MousePos);
            if (Vector3.Distance(worldPos, newVector) > 0.05f)
                _isMoving = true;
            worldPos = newVector;

            Vector3 resPos = new Vector3(worldPos.x - _diffCords.x, worldPos.y - _diffCords.y, worldPos.z);
            view.Move(new Vector3(resPos.x, resPos.y, 10));
            yield return null;
        }
        Drop(view);
    }
    private void Drop(IControllable binder)
    {
        _isMoving = false;
        binder.Drop();
    }
    private void SetDraggingFalse()
    {
        _isDragging = false;
    }
}
