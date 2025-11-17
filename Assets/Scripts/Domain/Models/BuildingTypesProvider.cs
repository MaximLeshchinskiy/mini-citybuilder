using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain
{
    public class BuildingTypesProvider
    {
        private readonly Dictionary<string, BuildingType> _buildingTypes
            ;

        public BuildingTypesProvider(List<BuildingType> buildingTypes)
        {
            _buildingTypes = buildingTypes.ToDictionary(it => it.Name, it => it);
        }
        
        
        public BuildingType GetBuildingType(string name)
        {
            return !_buildingTypes.TryGetValue(name, out var type) ? throw new Exception($"Missing type {name}") : type;
        }
    }
}