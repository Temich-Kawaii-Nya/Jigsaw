public interface IPuzzleGenerator
{
    PuzzleElementToGenerate[] PuzzlesToGenerate { get; }
    void Generate();    
}
