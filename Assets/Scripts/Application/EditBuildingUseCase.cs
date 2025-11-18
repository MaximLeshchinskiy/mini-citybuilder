using Domain;
using R3;
using VContainer;

namespace Application
{
    public class EditBuildingUseCase : IEditBuildingUseCase
    {
        [Inject] private GameState _gameState;

        public Subject<Building> BuildingDestroyed { get; } = new();

        public Building GetBuildingAtPos(GridPos pos)
        {
            return _gameState.CityGrid.GetBuildingInCell(pos);
        }

        public void DestroyBuildingAtPosition(GridPos gridPosHandled)
        {
            var building = _gameState.CityGrid.GetBuildingInCell(gridPosHandled);
            _gameState.CityGrid.Buildings.Remove(gridPosHandled);
            BuildingDestroyed.OnNext(building);
        }
    }
}