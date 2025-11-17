using System.Collections;
using System.Collections.Generic;
using Domain;
using R3;
using UnityEngine;

namespace Application
{
    public interface IPlaceBuildingUseCase
    {
        ReactiveProperty<Vector3> PlacingPosition { get; }
        IEnumerable<(BuildingType, int)> BuildingsAvailable { get; }
        public ReactiveProperty<Building> BuildingMoved { get; }
        public void CreateBuilding((BuildingType, int) buildingData);
    }
}