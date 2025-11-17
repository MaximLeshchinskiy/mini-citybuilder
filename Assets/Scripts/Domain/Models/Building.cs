
namespace Domain
{
    public class Building
    {
        public readonly BuildingType Type;
        public int Level;

        public Building(BuildingType type)
        {
            Type = type;
            Level = 0;
        }

        public int GetTickIncome()
        {
            return this.Type.Levels[this.Level].IncomePerTick;
        }
    }
}
