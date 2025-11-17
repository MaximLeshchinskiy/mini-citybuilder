using Domain;
using UnityEngine;
using VContainer;

namespace Presentation
{
    public class BuildingViewFactory : IBuildingViewFactory
    {
        [Inject] private IBuildingViewPrefabResolver _buildingViewPrefabResolver;
        
        public BuildingView CreateBuildingView(Building building)
        {
            var prefab = _buildingViewPrefabResolver.GetBuildingViewPrefab(building.Type, building.Level);
            return Object.Instantiate(prefab);
        }
    }
}