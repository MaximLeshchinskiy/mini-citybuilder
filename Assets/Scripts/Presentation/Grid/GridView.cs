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
        private readonly Dictionary<Building, BuildingView> _buildingViews = new();
        private readonly CompositeDisposable _compositeDisposable = new();

        private readonly Dictionary<GridPos, GridCellView> _gridCellViews = new();
        private GridPos _buildingHandledGridPos;
        [Inject] private IBuildingViewFactory _buildingViewFactory;

        private BuildingView _buildingViewHandled;
        private GridCellView _cellHandled;
        [Inject] private IEditBuildingUseCase _editBuildingUseCase;
        [Inject] private IPublisher<GridPosSelected> _gridPosSelected;
        [Inject] private IInputService _inputService;

        [Inject] private IInstantiateGridUseCase _instantiateGridUseCase;
        [Inject] private IPlaceBuildingUseCase _placeBuildingUseCase;

        private void OnDestroy()
        {
            _compositeDisposable.Dispose();
        }

        [Inject]
        private void OnPostInject()
        {
            PopulateGrid();

            _inputService.MouseLeftClick.Subscribe(HandleCellClick).AddTo(_compositeDisposable);
            _inputService.MouseWorldPosition.Subscribe(HandleBuildingMovement).AddTo(_compositeDisposable);
            _placeBuildingUseCase.BuildingBeingMoved.Subscribe(HandleBuildingPlacementStart)
                .AddTo(_compositeDisposable);
            _editBuildingUseCase.BuildingDestroyed.Subscribe(HandleBuildingDestroy).AddTo(_compositeDisposable);
            _editBuildingUseCase.BuildingUpgraded.Subscribe(HandleBuildingUpgrade).AddTo(_compositeDisposable);
        }

        private void HandleBuildingUpgrade(Building building)
        {
            var buildingView = _buildingViews[building];
            buildingView.PlayUpgradeFx();
            _buildingViews.Remove(building);
            Destroy(buildingView.gameObject);
            CreateBuildingAtCell(building, _cellHandled);
        }

        private void HandleBuildingDestroy(Building building)
        {
            var buildingView = _buildingViews[building];
            buildingView.PlayDestroyFx();
            _buildingViews.Remove(building);
            Destroy(buildingView.gameObject);
            _buildingViewHandled = null;
            _cellHandled?.SetNormalMode();
            _cellHandled = null;
        }


        private void HandleCellClick(Unit _)
        {
            var targetPosition = GetGridPosition(_inputService.MouseWorldPosition.Value);
            if (!targetPosition.HasValue) return;

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
            for (var j = 0; j < _instantiateGridUseCase.Width; j++)
            {
                var cellView = Instantiate(gridCellViewPrefab, transform);
                _gridCellViews.Add(new GridPos(i, j), cellView);
                cellView.transform.localPosition = new Vector3(j * gridCellSize, 0, i * gridCellSize);
                var building = _instantiateGridUseCase.GetBuildingAtCell(i, j);
                if (building != null) CreateBuildingAtCell(building, cellView);
            }
        }

        private void CreateBuildingAtCell(Building building, GridCellView cellView)
        {
            var buildingView = _buildingViewFactory.CreateBuildingView(building);
            _buildingViews.Add(building, buildingView);
            buildingView.transform.SetParent(cellView.transform);
            buildingView.transform.localPosition = Vector3.zero;
        }

        private void HandleBuildingMovement(Vector3 mouseWorldPosition)
        {
            if (!_buildingViewHandled) return;

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
            if (worldPosition.x < 0 ||
                worldPosition.x > gridCellSize * _instantiateGridUseCase.Width || //todo can it be updated
                worldPosition.z < 0 || worldPosition.z > gridCellSize * _instantiateGridUseCase.Height)
                return null;
            var z = (int)(worldPosition.x / gridCellSize + gridCellSize / 2f);
            var x = (int)(worldPosition.z / gridCellSize + gridCellSize / 2f);
            return new GridPos(x, z);
        }
    }
}