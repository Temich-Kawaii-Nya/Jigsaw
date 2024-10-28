using ObservableCollections;

public class WorldGameplayRootViewModel
{
    public readonly IObservableCollection<PuzzleViewModel> AllPuzzles;
    public readonly IObservableCollection<PuzzleGroupViewModel> AllGroups;

    public WorldGameplayRootViewModel(PuzzleService puzzlesService)
    {
        AllPuzzles = puzzlesService.AllPuzzles;
        AllGroups = puzzlesService.AllGroups; 
    }
}
