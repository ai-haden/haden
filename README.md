# haden

The Framework mobile AI.

## Working here

`dotnet run --project Haden.NxtRemote\Haden.NxtRemote.csproj`

## Embodied Light-Seeker Decision Tree Export

The branch-01/branch-02 embodied model can be exported into a `TreeNode` view for explainability and paper figures.

- Model source: `Haden.Library/Algorithm/IdealEmbodiedLightSeeker.cs`
- Export adapter: `Haden.Library/Algorithm/IdealEmbodiedDecisionTreeAdapter.cs`
- Paper section (LaTeX): `docs/paper/light_seeker_branch12.tex`

Use this when you want a human-readable summary of learned `experiment -> result` interactions.
It is intended for interpretation and analysis, not as the primary control loop.

Example:

```csharp
using Haden.Library.Algorithm;
using Haden.Library.DecisionTree;

var seeker = new IdealEmbodiedLightSeeker(
    experiments: new[] { "scan-left", "scan-right" },
    initialExperiment: "scan-left");

seeker.SetResultValence("dark", -1.0);
seeker.SetResultValence("bright", 1.0);

seeker.Step(exp => exp == "scan-left" ? "dark" : "bright");
seeker.Step(exp => exp == "scan-left" ? "dark" : "bright");

TreeNode tree = IdealEmbodiedDecisionTreeAdapter.Export(seeker);
string html = tree.ToHtmlTree();
```

## RL Test Commands

- `dotnet test Haden.Tests\Haden.Tests.csproj --filter "FullyQualifiedName~Haden.Tests.RewardTests" --logger "console;verbosity=detailed"`
- `dotnet test Haden.Tests\Haden.Tests.csproj --filter "FullyQualifiedName~Haden.Tests.IdealBranch12LightSeekerTests" --logger "console;verbosity=detailed"`
- `dotnet test Haden.Tests\Haden.Tests.csproj --filter "FullyQualifiedName~Haden.Tests.IdealEmbodiedDecisionTreeAdapterTests" --logger "console;verbosity=detailed"`
- `dotnet test Haden.Tests\Haden.Tests.csproj --filter "FullyQualifiedName~Haden.Tests.BaseLearnerTests" --logger "console;verbosity=detailed"`
- `dotnet test Haden.Tests\Haden.Tests.csproj --filter "FullyQualifiedName~Haden.Tests.WhirlEngineTests" --logger "console;verbosity=detailed"`
- `dotnet test Haden.Tests\Haden.Tests.csproj --logger "console;verbosity=detailed"`
