using System.Collections.Generic;
using Domain;
using R3;

namespace Application
{
    public interface IPlaceBuildingUseCase
    {
        IEnumerable<(BuildingType, int)> BuildingsAvailable { get; }
        public void CreateBuilding((BuildingType, int) buildingData);
        public ReactiveProperty<Building> BuildingBeingMoved { get; }
        public void StartBuildingMovement(Building buildingData);
        bool CanPlaceBuilding(GridPos targetPosition);
        void PlaceMovedBuilding(GridPos position);
    }
}