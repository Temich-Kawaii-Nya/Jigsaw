using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PuzzleGroupEntity : Entity
{
    public Vector3 position;
    public int rotation;
    public List<int> puzzlesID = new();
    public PuzzleGroupEntity(int id, Vector3 position, int rotation = 0)
    {
        this.id = id;
        this.position = position;
        this.rotation = rotation;
    }
}

