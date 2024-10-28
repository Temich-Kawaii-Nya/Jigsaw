using UnityEngine;

public interface IControllable
{
    PuzzleGroupBinder Parent { get; set; }
    Vector3 Position { get; }
    void Move(Vector3 pos);
    void Rotate();
    void Drop();
}
