using Domain;
using VContainer;

namespace Application
{
    public class InstantiateGridUseCase : IInstantiateGridUseCase
    {
        [Inject] BuildingTypesProvider _buildingTypesProvider;
        [Inject] GameState _gameState;
        
        public int Width { get; } = 5;
        public int Height { get; } = 5;
        public Building GetBuildingAtCell(int x, int y) => _gameState.CityGrid.GetBuildingInCell(new GridPos(x, y)); 
    }
}
