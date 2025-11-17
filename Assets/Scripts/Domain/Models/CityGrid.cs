using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain
{
    public class CityGrid 
    {
        public int Width;
        public int Height;
        public Dictionary<GridPos, Building> Buildings = new();

        public int GetTickIncome()
        {
            return this.Buildings.Values.Sum(building => building.GetTickIncome());
        }
        
        public void PlaceBuilding(Building building, GridPos pos)
        {
            if (!this.Buildings.TryAdd(pos, building))
            {
                throw new Exception("Building already exists at position " + pos);
            }
        }
        
        public bool IsCellOccupied(GridPos pos)
        {
            return this.Buildings.ContainsKey(pos);
        }
    }
}
