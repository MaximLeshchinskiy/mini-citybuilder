using Domain;

namespace Presentation
{
    public interface IBuildingViewFactory
    {
        BuildingView CreateBuildingView(Building building);
    }
}