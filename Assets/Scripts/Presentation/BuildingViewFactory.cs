using Domain;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Presentation
{
    public class BuildingViewFactory : IBuildingViewFactory
    {
        [Inject] private IBuildingViewPrefabResolver _buildingViewPrefabResolver;
        [Inject] private LifetimeScope _scope;
        
        public BuildingView CreateBuildingView(Building building)
        {
            var prefab = _buildingViewPrefabResolver.GetBuildingViewPrefab(building.Type, building.Level);
            var sub = _scope.CreateChild(insstalation =>
            {
                insstalation.RegisterInstance(building);
            });
            return sub.Container.Instantiate(prefab);
        }
    }
}