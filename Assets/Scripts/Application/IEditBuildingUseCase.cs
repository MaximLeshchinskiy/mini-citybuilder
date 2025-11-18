using Domain;
using R3;

namespace Application
{
    public interface IEditBuildingUseCase
    {
        Subject<Building> BuildingDestroyed { get; }
        Subject<Building> BuildingUpgraded { get; }
        Building GetBuildingAtPos(GridPos pos);
        void DestroyBuildingAtPosition(GridPos gridPosHandled);
        void UpgradeBuildingAtPosition(GridPos gridPosHandled);
        bool CanUpgradeBuildingAtPosition(GridPos gridPosHandled);
    }
}