using Domain;

namespace Presentation
{
    public interface IBuildingViewPrefabResolver
    {
        public BuildingView GetBuildingViewPrefab(BuildingType buildingType, int level);
    }
}