using Domain;
using VContainer;

namespace Application
{
   
    public class TickRunner
    {
        [Inject] private GameState _gameState;

        public void RunTick()
        {
            _gameState.ProcessEconomyTick();
        }
    }
}
