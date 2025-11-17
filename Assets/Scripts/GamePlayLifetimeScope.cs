using System.Collections.Generic;
using Application;
using Domain;
using Infrastructure.Gameplay;
using Presentation;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Tmp
{
    public class GamePlayLifetimeScope : LifetimeScope
    {
        [SerializeField] private BuildingsConfigProvider buildingsConfigProvider;
         
        protected override void Configure(IContainerBuilder builder)
        {
           
            
            buildingsConfigProvider.Initialize();
            var buildingTypesProvider = new BuildingTypesProvider(buildingsConfigProvider.BuildingTypes);
            builder.RegisterInstance(buildingTypesProvider);
            builder.RegisterInstance<IBuildingViewPrefabResolver>(buildingsConfigProvider);
            builder.Register<IBuildingViewFactory, BuildingViewFactory>(Lifetime.Singleton);
            builder.Register<IInstantiateGridUseCase,InstantiateGridUseCase>(Lifetime.Singleton);
        }

    }
    
    
    
}
