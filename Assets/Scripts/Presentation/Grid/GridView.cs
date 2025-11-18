using System;
using System.Collections.Generic;
using Application;
using Domain;
using Infrastructure;
using MessagePipe;
using R3;
using UnityEngine;

using VContainer;

namespace Presentation.Grid
{
    public class GridView : MonoBehaviour
    {
        [SerializeField] private GridCellView gridCellViewPrefab;
        [SerializeField] private float gridCellSize;
        
        [Inject] private IInstantiateGridUseCase _instantiateGridUseCase;
        [Inject] private IPlaceBuildingUseCase _placeBuildingUseCase;
        [Inject] private IBuildingViewFactory _buildingViewFactory;
        [Inject] private IInputService _inputService;
        [Inject] private IPublisher<GridPosSelected> _gridPosSelected;
        
        private readonly Dictionary<GridPos, GridCellView> _gridCellViews = new();
        private readonly Dictionary<Building, BuildingView> _buildingViews = new();
        private readonly CompositeDisposable _compositeDisposable = new();
      
        private BuildingView _buildingViewHandled;
        private GridPos _buildingHandledGridPos;
        private GridCellView _cellHandled;
        
        [Inject]
        private void OnPostInject()
        {
            PopulateGrid();

            _inputService.MouseLeftClick.Subscribe(HandleCellClick).AddTo(_compositeDisposable);
            _inputService.MouseWorldPosition.Subscribe(HandleBuildingMovement).AddTo(_compositeDisposable);
            _placeBuildingUseCase.BuildingBeingMoved.Subscribe(HandleBuildingPlacementStart).AddTo(_compositeDisposable);
        }
        

        private void HandleCellClick(Unit _)
        {
            var targetPosition = GetGridPosition(_inputService.MouseWorldPosition.Value);
            if (!targetPosition.HasValue)
            {
                return;
            }
            
            if (_buildingViewHandled == null)
            {
                _cellHandled?.SetNormalMode();
                _cellHandled = _gridCellViews[targetPosition.Value];
                _cellHandled.SetSelected();
                _gridPosSelected.Publish(new GridPosSelected(targetPosition.Value));
                return;
            }
            
            
            if (_placeBuildingUseCase.CanPlaceBuilding(targetPosition.Value))
            {
                _buildingViews.TryAdd(_placeBuildingUseCase.BuildingBeingMoved.Value, _buildingViewHandled);
                _buildingViewHandled.transform.SetParent(_cellHandled.transform);
                _buildingViewHandled.transform.localPosition = Vector3.zero;
                _buildingViewHandled.SetNormalMode();
                _buildingViewHandled = null;
                _placeBuildingUseCase.PlaceMovedBuilding(targetPosition.Value);
            }
        }
        
        private void PopulateGrid()
        {
            for (var i = 0; i < _instantiateGridUseCase.Height; i++)
            {
                for (var j = 0; j < _instantiateGridUseCase.Width; j++)
                {
                    var cellView = Instantiate(gridCellViewPrefab, transform);
                    _gridCellViews.Add(new GridPos(i, j), cellView);
                    cellView.transform.localPosition = new Vector3(j * gridCellSize, 0, i * gridCellSize);
                    var building = _instantiateGridUseCase.GetBuildingAtCell(i, j);
                    if (building != null)
                    {
                        var buildingView = _buildingViewFactory.CreateBuildingView(building);
                        _buildingViews.Add(building, buildingView);
                        buildingView.transform.SetParent(cellView.transform);
                        buildingView.transform.localPosition = cellView.transform.position;
                    }
                }
            }
        }
        
        private void HandleBuildingMovement(Vector3 mouseWorldPosition)
        {
            if (!_buildingViewHandled)
            {
                return;
            }
            
            _buildingViewHandled.transform.position = mouseWorldPosition;
            _cellHandled?.SetNormalMode();
            _cellHandled = null;
            var targetPosition = GetGridPosition(_inputService.MouseWorldPosition.Value);
            if (targetPosition.HasValue)
            {
                _cellHandled = _gridCellViews[targetPosition.Value];
                _cellHandled.SetHighlightMode(_placeBuildingUseCase.CanPlaceBuilding(targetPosition.Value));
            }
        }

        private void HandleBuildingPlacementStart(Building building)
        {
            if (building == null)
            {
                _buildingViewHandled?.SetNormalMode();
                _buildingViewHandled = null;
                return;
            }
            _buildingViewHandled = _buildingViews.TryGetValue(building, out var view)
                ? view
                : _buildingViewFactory.CreateBuildingView(building);
            _buildingViewHandled.transform.position = _inputService.MouseWorldPosition.Value;
            _buildingViewHandled.SetMovedMode();
            
        }

        private GridPos? GetGridPosition(Vector3 worldPosition)
        {
            if (worldPosition.x < 0 || worldPosition.x > gridCellSize * _instantiateGridUseCase.Width || //todo can it be updated
                worldPosition.z < 0 || worldPosition.z > gridCellSize * _instantiateGridUseCase.Height)
            {
                return null;
            }
            var z = (int)(worldPosition.x / gridCellSize);
            var x = (int)(worldPosition.z / gridCellSize);
            return new GridPos(x, z);
        }
        
        private void OnDestroy()
        {
            _compositeDisposable.Dispose();
        }
    }
}
