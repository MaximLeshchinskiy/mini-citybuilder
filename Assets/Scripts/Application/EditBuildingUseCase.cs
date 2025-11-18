using Domain;
using R3;
using VContainer;

namespace Application
{
    public class EditBuildingUseCase : IEditBuildingUseCase
    {
        [Inject] private GameState _gameState;

        public Subject<Building> BuildingDestroyed { get; } = new();
        public Subject<Building> BuildingUpgraded { get; } = new();

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

        public void UpgradeBuildingAtPosition(GridPos gridPosHandled)
        {
            var building = _gameState.CityGrid.GetBuildingInCell(gridPosHandled);
            building.Level++;
            BuildingUpgraded.OnNext(building);
        }

        public bool CanUpgradeBuildingAtPosition(GridPos gridPosHandled)
        {
            var building = _gameState.CityGrid.GetBuildingInCell(gridPosHandled);
            return building.Level < building.Type.Levels.Count - 1;
        }
    }
}