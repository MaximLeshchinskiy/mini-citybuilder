
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
        
        protected override void AfterInitialized()
        {
            View.Hide();
            CompositeDisposable.Add(_gridPosSelected.Subscribe(OnGridPosSelected));
            View.MoveButtonClicked.Subscribe(_ =>
            {
                _placeBuildingUseCase.StartBuildingMovement(_editBuildingUseCase.GetBuildingAtPos(_gridPosHandled));
                View.Hide();
            }).AddTo(CompositeDisposable);
            

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
                View.Show();
            }
        }
    }
}