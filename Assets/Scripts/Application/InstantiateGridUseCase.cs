using Domain;
using VContainer;

namespace Application
{
    public class InstantiateGridUseCase : IInstantiateGridUseCase
    {
        [Inject] private BuildingTypesProvider _buildingTypesProvider;
        [Inject] private GameState _gameState;

        public int Width { get; } = 5;
        public int Height { get; } = 5;

        public Building GetBuildingAtCell(int x, int y)
        {
            return _gameState.CityGrid.GetBuildingInCell(new GridPos(x, y));
        }
    }
}