using Domain;
using JetBrains.Annotations;
using VContainer;

namespace Application
{
    public interface IInstantiateGridUseCase
    {
        int Width { get; }
        int Height { get;  }
        [CanBeNull] Building GetBuildingAtCell(int i, int i1);
    }
    
    
    public class InstantiateGridUseCase : IInstantiateGridUseCase
    {
        [Inject] BuildingTypesProvider _buildingTypesProvider;
        
        public int Width { get; } = 5;
        public int Height { get; } = 5;
        public Building GetBuildingAtCell(int x, int y)
        {
            if (x == 0 && y == 0)
            {
                return new Building(_buildingTypesProvider.GetBuildingType("House"));
            }

            return null;
        }
    }
}
