using R3;
using UnityEngine;

public class UIGameplayRootBinder : MonoBehaviour
{
    private Subject<Unit> _exitSceneSignalSubj;

    public void HandleGoToMainMenuButtonClicked()
    {
        _exitSceneSignalSubj?.OnNext(Unit.Default);
    }

    public void Bind(Subject<Unit> exitSceneSignalSubj)
    {
        _exitSceneSignalSubj = exitSceneSignalSubj;
    }
}
