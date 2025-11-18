using System;
using System.Collections.Generic;
using System.Linq;
using Domain;
using Infrastructure;
using R3;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Application
{
    public class PlaceBuildingUseCase : IPlaceBuildingUseCase, IInitializable, IDisposable
    {
        private readonly CompositeDisposable _compositeDisposable = new();

        [Inject] private GameState _gameState;
        private Building _handlingBuildingPlacement;
        [Inject] private IInputService _inputService;

        public void Dispose()
        {
            _compositeDisposable.Dispose();
        }

        public void Initialize()
        {
            //todo remove if empty
        }

        public IEnumerable<(BuildingType, int)> BuildingsAvailable => _gameState.BuildingsAvailable.ToList();
        public ReactiveProperty<Building> BuildingBeingMoved { get; } = new();

        public void CreateBuilding((BuildingType, int) buildingData)
        {
            if (BuildingBeingMoved.Value != null) return;
            StartBuildingMovement(new Building(buildingData.Item1, buildingData.Item2));
        }

        public void StartBuildingMovement(Building building)
        {
            BuildingBeingMoved.Value = building;
        }

        public bool CanPlaceBuilding(GridPos targetPosition)
        {
            return _gameState.CityGrid.GetBuildingInCell(targetPosition) == null;
        }

        public void PlaceMovedBuilding(GridPos position)
        {
            Debug.Log($"Place building at {position} ");
            _gameState.CityGrid.PlaceBuilding(BuildingBeingMoved.Value, position);
            BuildingBeingMoved.Value = null;
        }
    }
}