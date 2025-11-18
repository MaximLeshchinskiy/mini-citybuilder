using System.Collections.Generic;

namespace Domain
{
    public class GameState //todo interfaces 
    {
        public List<(BuildingType, int)> BuildingsAvailable;
        public CityGrid CityGrid;
        public int Gold;


        public void ProcessEconomyTick()
        {
            Gold += CityGrid.GetTickIncome();
        }
    }
}