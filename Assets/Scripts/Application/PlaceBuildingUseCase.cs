using System;
using System.Collections.Generic;
using System.Linq;
using Domain;
using Infrastructure;
using Presentation;
using R3;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Application
{
    public class PlaceBuildingUseCase : IPlaceBuildingUseCase, IInitializable, IDisposable
    {
        [Inject] private GameState _gameState;
        [Inject] private IInputService _inputService;
        [Inject] private IGridPositionProvider _gridPositionProvider;

        public ReactiveProperty<Vector3> PlacingPosition => _inputService.MouseWorldPosition;
        public IEnumerable<(BuildingType, int)> BuildingsAvailable => _gameState.BuildingsAvailable.ToList();
        public ReactiveProperty<Building> BuildingMoved { get; } = new();
        
        private readonly CompositeDisposable _compositeDisposable = new();
        private Building _handlingBuildingPlacement;

        public void CreateBuilding((BuildingType, int) buildingData)
        {
            if (BuildingMoved.Value != null)
            {
                return;
            }
            BuildingMoved.Value = new Building(buildingData.Item1, buildingData.Item2);
        }

        public void Initialize()
        {
            _inputService.MouseLeftClick.Subscribe(_ =>
            {
                if (BuildingMoved.Value != null)
                {
                    var gridPosition = _gridPositionProvider.GetGridPosition(_inputService.MouseWorldPosition.Value);
                    if (gridPosition != null && _gameState.CityGrid.GetBuildingInCell(gridPosition.Value) == null)
                    {
                        _gameState.CityGrid.PlaceBuilding(BuildingMoved.Value, gridPosition.Value);
                    }
                }
            }).AddTo(_compositeDisposable);
        }

        public void Dispose()
        {
            _compositeDisposable.Dispose();
        }
    }
}
