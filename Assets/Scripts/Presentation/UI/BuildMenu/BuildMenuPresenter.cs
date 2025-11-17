using System;
using Application;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Presentation.UI.BuildMenu
{
    public class BuildMenuPresenter : IDisposable, IInitializable
    {
        [Inject] private IPlaceBuildingUseCase placeBuildingUseCase;
        [Inject] private IBuildMenuView buildMenuView;
        
        private readonly CompositeDisposable _compositeDisposable = new();

        public void Dispose()
        {
            _compositeDisposable.Dispose();
        }

        public void Initialize()
        {
            InitializeView().Forget(Debug.LogError);
        }
        
        private async UniTask InitializeView()
        {
            await buildMenuView.Init();
            foreach (var buildingType in placeBuildingUseCase.BuildingsAvailable)
            {
                buildMenuView.AddButton(buildingType.Item1.Name, buildingType.Item2).Subscribe(_ => 
                {
                    placeBuildingUseCase.CreateBuilding(buildingType);
                }).AddTo(_compositeDisposable);
            }
        }
    }
}