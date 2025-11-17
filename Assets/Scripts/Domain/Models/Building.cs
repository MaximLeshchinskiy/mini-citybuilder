
namespace Domain
{
    public class Building
    {
        public readonly BuildingType Type;
        public int Level;

        public Building(BuildingType type, int level)
        {
            Type = type;
            Level = 0;
        }

        public int GetTickIncome()
        {
            return Type.Levels[Level].IncomePerTick;
        }
    }
}
