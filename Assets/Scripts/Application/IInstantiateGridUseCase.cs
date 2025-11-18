using Domain;
using JetBrains.Annotations;

namespace Application
{
    public interface IInstantiateGridUseCase
    {
        int Width { get; }
        int Height { get; }

        [CanBeNull]
        Building GetBuildingAtCell(int i, int i1);
    }
}