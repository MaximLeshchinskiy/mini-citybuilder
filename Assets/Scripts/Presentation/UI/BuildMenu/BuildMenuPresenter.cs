using Application;
using Presentation.UI.Lib;
using R3;
using VContainer;

namespace Presentation.UI.BuildMenu
{
    public class BuildMenuPresenter : APresenter<IBuildMenuView>
    {
        [Inject] private IBuildMenuView buildMenuView;
        [Inject] private IPlaceBuildingUseCase placeBuildingUseCase;


        protected override void AfterInitialized()
        {
            foreach (var buildingType in placeBuildingUseCase.BuildingsAvailable)
                buildMenuView.AddButton(buildingType.Item1.Name, buildingType.Item2).Subscribe(_ =>
                {
                    placeBuildingUseCase.CreateBuilding(buildingType);
                }).AddTo(CompositeDisposable);
        }
    }
}