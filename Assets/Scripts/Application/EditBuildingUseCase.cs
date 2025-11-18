using Domain;
using VContainer;

namespace Application
{
    public class EditBuildingUseCase : IEditBuildingUseCase
    {
        [Inject] private GameState _gameState;
        
        public Building GetBuildingAtPos(GridPos pos)
        {
            return _gameState.CityGrid.GetBuildingInCell(pos);
        }
    }
}