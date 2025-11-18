
using Application;
using Domain;
using MessagePipe;
using Presentation.Grid;
using Presentation.UI.Lib;
using R3;
using VContainer;

namespace Presentation.UI.BuildingEditMenu
{
    public class BuildingMenuPresenter : APresenter<IBuildingEditMenuView>
    {
        [Inject] ISubscriber<GridPosSelected> _gridPosSelected;
        [Inject] IEditBuildingUseCase _editBuildingUseCase;
        [Inject] IPlaceBuildingUseCase _placeBuildingUseCase;

        private GridPos _gridPosHandled;
        private readonly ReactiveProperty<int> _buildingLevel = new();
        private readonly ReactiveProperty<string> _buildingName = new();
        private readonly ReactiveProperty<bool> _canUpgrade = new();
        
        protected override void AfterInitialized()
        {
            View.Hide();
            CompositeDisposable.Add(_gridPosSelected.Subscribe(OnGridPosSelected));
            View.MoveButtonClicked.Subscribe(_ =>
            {
                _placeBuildingUseCase.StartBuildingMovement(_editBuildingUseCase.GetBuildingAtPos(_gridPosHandled));
                View.Hide();
            }).AddTo(CompositeDisposable);
            View.DestroyButtonClicked.Subscribe(_ =>
            {
                _editBuildingUseCase.DestroyBuildingAtPosition(_gridPosHandled);
                View.Hide();
            }).AddTo(CompositeDisposable);
            View.UpgradeButtonClicked.Subscribe(_ =>
            {
                _editBuildingUseCase.UpgradeBuildingAtPosition(_gridPosHandled);
                _canUpgrade.Value = _editBuildingUseCase.CanUpgradeBuildingAtPosition(_gridPosHandled);
                _buildingLevel.Value++;
            }).AddTo(CompositeDisposable);
            View.BindBuildingLevel(_buildingLevel);
            View.BindBuildingName(_buildingName);
            View.BindUpgradeButton(_canUpgrade);

        }
        
        private void OnGridPosSelected(GridPosSelected gridPosSelected)
        {
            if (_editBuildingUseCase.GetBuildingAtPos(gridPosSelected.GridPos) == null)
            {
                View.Hide();
            }
            else
            {
                _gridPosHandled = gridPosSelected.GridPos;
                var building = _editBuildingUseCase.GetBuildingAtPos(_gridPosHandled);
                _canUpgrade.Value = _editBuildingUseCase.CanUpgradeBuildingAtPosition(_gridPosHandled);
                _buildingLevel.Value = building.Level;
                _buildingName.Value = building.Type.Name;
                View.Show();
            }
        }
    }
}