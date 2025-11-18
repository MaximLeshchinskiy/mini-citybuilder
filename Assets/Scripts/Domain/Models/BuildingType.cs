using System.Collections.Generic;

namespace Domain
{
    public class BuildingType
    {
        public readonly List<BuildingLevel> Levels;
        public readonly string Name;

        public BuildingType(string name, List<BuildingLevel> levels)
        {
            Levels = levels;
            Name = name;
        }
    }
}