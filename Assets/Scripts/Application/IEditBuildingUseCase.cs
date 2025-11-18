using Domain;

namespace Application
{
    public interface IEditBuildingUseCase
    {
        Building GetBuildingAtPos(GridPos pos);
    }
}