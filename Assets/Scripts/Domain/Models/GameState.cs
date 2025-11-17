namespace Domain
{
    public class GameState
    {
        public int Gold;
        public CityGrid CityGrid;
        
        

        public void ProcessEconomyTick()
        {
            this.Gold += this.CityGrid.GetTickIncome();
        }
    }
}
