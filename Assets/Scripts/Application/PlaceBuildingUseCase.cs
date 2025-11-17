using System.Collections.Generic;
using System.Linq;
using Domain;
using VContainer;

namespace Application
{
    public class PlaceBuildingUseCase : IPlaceBuildingUseCase 
    {
        [Inject] private GameState gameState;
        
        public IEnumerable<BuildingType> BuildingsAvailable => gameState.BuildingsAvailable.Select(it => it.Item1).ToList();
    }
}
