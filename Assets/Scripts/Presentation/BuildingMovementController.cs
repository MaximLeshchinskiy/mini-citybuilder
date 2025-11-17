using System;
using Application;
using Domain;
using R3;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Presentation
{
    public class BuildingMovementController : IInitializable, IDisposable
    {
        [Inject] private IPlaceBuildingUseCase _placeBuildingUseCase;
        [Inject] private IBuildingViewFactory _buildingViewFactory;
        
        private readonly CompositeDisposable _compositeDisposable = new();
        private BuildingView _buildingHandled;
        
        public void Initialize()
        {
            _placeBuildingUseCase.BuildingMoved.Subscribe(HandleBuildingPlacement).AddTo(_compositeDisposable);
            _placeBuildingUseCase.PlacingPosition.Subscribe(HandleBuildingMovement).AddTo(_compositeDisposable);
        }
        
        private void HandleBuildingMovement(Vector3 mouseWorldPosition)
        {
            if (!_buildingHandled)
            {
                return;
            }
            _buildingHandled.transform.position = mouseWorldPosition;
        }

        private void HandleBuildingPlacement(Building building)
        {
            if (building == null)
            {
                return;
            }
            //todo move case 
            _buildingHandled = _buildingViewFactory.CreateBuildingView(building);
            _buildingHandled.SetGhostMode();
            _buildingHandled.transform.position = _placeBuildingUseCase.PlacingPosition.Value;
        }
        
        
        public void Dispose()
        {
            _compositeDisposable.Dispose();
        }
    }
}