namespace Domain
{
    public class BuildingLevel
    {
        public readonly int Cost;
        public readonly int IncomePerTick;
        
        public BuildingLevel(int cost, int incomePerTick)
        {
            Cost = cost;
            IncomePerTick = incomePerTick;
        }
    }
}
