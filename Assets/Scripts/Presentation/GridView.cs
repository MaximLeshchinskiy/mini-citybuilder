using System;
using System.Collections.Generic;
using Application;
using Domain;
using R3;
using UnityEngine;

using VContainer;

namespace Presentation
{
    public class GridView : MonoBehaviour, IGridPositionProvider
    {
        [SerializeField] private GridCellView gridCellViewPrefab;
        [SerializeField] private float gridCellSize;
        
        [Inject] private IInstantiateGridUseCase _instantiateGridUseCase;
        [Inject] private IPlaceBuildingUseCase _placeBuildingUseCase;
        [Inject] private IBuildingViewFactory _buildingViewFactory;
        
        private readonly Dictionary<GridPos, GridCellView> _gridCellViews = new();
        private readonly Dictionary<Building, BuildingView> _buildingViews = new();
        private readonly CompositeDisposable _compositeDisposable = new();
      
        private BuildingView _buildingHandled;
        private GridPos _buildingHandledGridPos;
        private GridCellView _cellHighlighted;
        
        [Inject]
        private void OnPostInject()
        {
            PopulateGrid();
            _placeBuildingUseCase.BuildingMoved.Subscribe(HandleBuildingPlacement).AddTo(_compositeDisposable);
            _placeBuildingUseCase.PlacingPosition.Subscribe(HandleBuildingMovement).AddTo(_compositeDisposable);
            _placeBuildingUseCase.BuildingPlaced.Subscribe(OnPlaced).AddTo(_compositeDisposable);
        }

        private void OnPlaced((Building, GridPos) placed)
        {
            _buildingViews.TryAdd(placed.Item1, _buildingHandled);
            _buildingHandled.transform.SetParent(_gridCellViews[placed.Item2].transform);
            _buildingHandled.transform.localPosition = Vector3.zero;
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
            if (!_buildingHandled)
            {
                return;
            }
            
            _buildingHandled.transform.position = mouseWorldPosition;
            _cellHighlighted?.SetNormalMode();
            _cellHighlighted = null;
            var targetPosition = GetGridPosition(_placeBuildingUseCase.PlacingPosition.Value);
            if (targetPosition.HasValue)
            {
                _cellHighlighted = _gridCellViews[targetPosition.Value];
                _cellHighlighted.SetHighlightMode(_placeBuildingUseCase.CanPlaceBuilding(targetPosition.Value));
            }
        }

        private void HandleBuildingPlacement(Building building)
        {
            if (building == null)
            {
                _buildingHandled?.SetNormalMode();
                _buildingHandled = null;
                return;
            }
            //todo move case 
            _buildingHandled = _buildingViewFactory.CreateBuildingView(building);
            _buildingHandled.transform.position = _placeBuildingUseCase.PlacingPosition.Value;
            _buildingHandled.SetMovedMode();
            
        }

        public GridPos? GetGridPosition(Vector3 worldPosition)
        {
            if (worldPosition.x < 0 || worldPosition.x > gridCellSize * _instantiateGridUseCase.Width || //todo can it be updated
                worldPosition.z < 0 || worldPosition.z > gridCellSize * _instantiateGridUseCase.Height)
            {
                return null;
            }
            var x = (int)(worldPosition.x / gridCellSize);
            var z = (int)(worldPosition.z / gridCellSize);
            return new GridPos(x, z);
        }
        
        private void OnDestroy()
        {
            _compositeDisposable.Dispose();
        }
    }
}
