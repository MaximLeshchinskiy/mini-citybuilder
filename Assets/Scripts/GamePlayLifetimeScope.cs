using System.Collections.Generic;
using Application;
using Domain;
using Infrastructure;
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
        
        [Header("Views")]
        [SerializeField] private BuildMenuView buildMenuView;
        [SerializeField] private GridView gridView;
         
        protected override void Configure(IContainerBuilder builder)
        {
           
            
            buildingsConfigProvider.Initialize();
            var buildingTypesProvider = new BuildingTypesProvider(buildingsConfigProvider.BuildingTypes);
            var gameState = new GameState()
            {
                BuildingsAvailable = new List<(BuildingType, int)>()
                {
                    (buildingsConfigProvider.BuildingTypes[0], 0),
                    (buildingsConfigProvider.BuildingTypes[1], 0),
                    (buildingsConfigProvider.BuildingTypes[2], 0),
                }
                ,
                CityGrid = new CityGrid(5, 5)
            };
            builder.RegisterInstance(gameState);
            builder.RegisterInstance(buildingTypesProvider);
            builder.RegisterInstance<IBuildingViewPrefabResolver>(buildingsConfigProvider);
            builder.Register<IBuildingViewFactory, BuildingViewFactory>(Lifetime.Singleton);
            
            
            BindUseCases(builder);
            BindStaticViews(builder);
            BindInfrastructure(builder);
            
        }

        private static void BindInfrastructure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<DesktopInputService>().As<IInputService>();
        }


        private void BindStaticViews(IContainerBuilder builder)
        {
            builder.RegisterInstance(buildMenuView).As<IBuildMenuView>();
            builder.RegisterEntryPoint<BuildMenuPresenter>();
            builder.RegisterEntryPoint<BuildingMovementController>();
            builder.RegisterInstance(gridView).As<IGridPositionProvider>();
        }

        private static void BindUseCases(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<PlaceBuildingUseCase>().As<IPlaceBuildingUseCase>();
            builder.Register<IInstantiateGridUseCase,InstantiateGridUseCase>(Lifetime.Singleton);
        }
    }
    
    
    
}
