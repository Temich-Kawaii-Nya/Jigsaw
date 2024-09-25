using UnityEngine;

public class GameplayEntryPoint : MonoBehaviour
{
    [SerializeField] private GameObject _sceneRootBinder;
    
    public void Run()
    {
        Debug.Log("Gameplay scene loaded");
    }
}
