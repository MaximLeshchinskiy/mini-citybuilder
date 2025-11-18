using System.Collections.Generic;

namespace Domain
{
    public class GameState //todo interfaces 
    {
        public int Gold;
        public CityGrid CityGrid;
        public List<(BuildingType, int)> BuildingsAvailable;
        

        public void ProcessEconomyTick()
        {
            this.Gold += this.CityGrid.GetTickIncome();
        }
    }
}
