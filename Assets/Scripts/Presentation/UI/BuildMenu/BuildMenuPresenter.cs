using System;
using Application;
using R3;
using VContainer;
using VContainer.Unity;

namespace Presentation.UI.BuildMenu
{
    public class BuildMenuPresenter : IDisposable, IInitializable
    {
        [Inject] private IPlaceBuildingUseCase placeBuildingUseCase;
        [Inject] private IBuildMenuView buildMenuView;
        private CompositeDisposable _compositeDisposable;


        public void Dispose()
        {
            _compositeDisposable.Dispose();
        }

        public void Initialize()
        {
            foreach (var buildingType in placeBuildingUseCase.BuildingsAvailable)
            {
                buildMenuView.AddButton(buildingType.Name).AddTo(_compositeDisposable);
            }
        }
    }
}