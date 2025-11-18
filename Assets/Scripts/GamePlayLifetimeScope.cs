using System.Collections.Generic;
using Application;
using Cysharp.Threading.Tasks;
using Domain;
using Infrastructure;
using Infrastructure.Gameplay;
using MessagePipe;
using Presentation;
using Presentation.Grid;
using Presentation.UI.BuildingEditMenu;
using Presentation.UI.BuildMenu;
using Presentation.UI.GoldCounter;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using IBuildMenuView = Presentation.UI.BuildMenu.IBuildMenuView;

namespace Tmp
{
    public class GamePlayLifetimeScope : LifetimeScope
    {
        [Header("configs")]
        [SerializeField] private BuildingsConfigProvider buildingsConfigProvider;
        [SerializeField] private GameplaySettings gameplaySettings;
        
        [Header("Views")]
        [SerializeField] private BuildMenuView buildMenuView;
        [SerializeField] private GridView gridView;
        [SerializeField] private BuildingEditMenuView buildingEditMenuView;
        [SerializeField] private GoldCounterView goldCounterView;
         
        protected override void Configure(IContainerBuilder builder)
        {
            BindConfigs(builder);

            var gameState = MocTestData();
            builder.RegisterInstance(gameState);
            
            builder.RegisterInstance<IBuildingViewPrefabResolver>(buildingsConfigProvider);
            builder.Register<IBuildingViewFactory, BuildingViewFactory>(Lifetime.Singleton);
            
            BindUseCases(builder);
            BindStaticViews(builder);
            BindInfrastructure(builder);
            RegisterMessages(builder);
            var a = this.GetCancellationTokenOnDestroy();
        }

        private void BindConfigs(IContainerBuilder builder)
        {
            buildingsConfigProvider.Initialize();
            var buildingTypesProvider = new BuildingTypesProvider(buildingsConfigProvider.BuildingTypes);
            builder.RegisterInstance(buildingTypesProvider);
            builder.RegisterInstance(gameplaySettings);
        }

        private GameState MocTestData()
        {
            var gameState = new GameState()
            {
                BuildingsAvailable = new List<(BuildingType, int)>()
                {
                    (buildingsConfigProvider.BuildingTypes[0], 0),
                    (buildingsConfigProvider.BuildingTypes[1], 0),
                    (buildingsConfigProvider.BuildingTypes[2], 0),
                },
                CityGrid = new CityGrid(5, 5)
            };
            return gameState;
        }

        private static void RegisterMessages(IContainerBuilder builder)
        {
            var options = builder.RegisterMessagePipe();
            builder.RegisterMessageBroker<GridPosSelected>(options);
        }

        private static void BindInfrastructure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<DesktopInputService>().As<IInputService>();
        }

        private void BindStaticViews(IContainerBuilder builder)
        {
            builder.RegisterInstance(buildMenuView).As<IBuildMenuView>();
            builder.RegisterEntryPoint<BuildMenuPresenter>();
            builder.RegisterInstance(buildingEditMenuView).As<IBuildingEditMenuView>();
            builder.RegisterEntryPoint<BuildingMenuPresenter>();
            builder.RegisterInstance(goldCounterView).As<IGoldCounterView>();
            builder.RegisterEntryPoint<GoldCounterPresenter>();
        }

        private static void BindUseCases(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<PlaceBuildingUseCase>().As<IPlaceBuildingUseCase>();
            builder.Register<IInstantiateGridUseCase,InstantiateGridUseCase>(Lifetime.Singleton);
            builder.RegisterEntryPoint<EditBuildingUseCase>().As<IEditBuildingUseCase>();
            builder.RegisterEntryPoint<EconomyService>().As<IEconomyService>();
        }
    }
}
