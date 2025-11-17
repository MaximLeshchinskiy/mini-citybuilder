using System.Collections.Generic;
using Application;
using Domain;
using Infrastructure.Gameplay;
using Presentation;
using Presentation.UI.BuildMenu;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Tmp
{
    public class GamePlayLifetimeScope : LifetimeScope
    {
        [Header("configs")]
        [SerializeField] private BuildingsConfigProvider buildingsConfigProvider;
        
        [Header("UI")]
        [SerializeField] private BuildMenuView buildMenuView;
         
        protected override void Configure(IContainerBuilder builder)
        {
           
            
            buildingsConfigProvider.Initialize();
            var buildingTypesProvider = new BuildingTypesProvider(buildingsConfigProvider.BuildingTypes);
            var gameState = new GameState()
            {
                BuildingsAvailable = new List<(BuildingType, int)>()
                {
                    (buildingsConfigProvider.BuildingTypes[0], 1),
                    (buildingsConfigProvider.BuildingTypes[1], 1),
                    (buildingsConfigProvider.BuildingTypes[2], 1),
                }
            };
            builder.RegisterInstance(gameState);
            builder.RegisterInstance(buildingTypesProvider);
            builder.RegisterInstance<IBuildingViewPrefabResolver>(buildingsConfigProvider);
            builder.Register<IBuildingViewFactory, BuildingViewFactory>(Lifetime.Singleton);
            
            
            BindUseCases(builder);
            BindStaticUI(builder);
        }

        private void BindStaticUI(IContainerBuilder builder)
        {
            builder.RegisterInstance(buildMenuView).As<IBuildMenuView>();
            builder.RegisterEntryPoint<BuildMenuPresenter>();
        }

        private static void BindUseCases(IContainerBuilder builder)
        {
            builder.Register<IPlaceBuildingUseCase, PlaceBuildingUseCase>(Lifetime.Singleton);
            builder.Register<IInstantiateGridUseCase,InstantiateGridUseCase>(Lifetime.Singleton);
        }
    }
    
    
    
}
