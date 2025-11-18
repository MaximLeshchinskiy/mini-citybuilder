using System;
using System.Collections.Generic;
using System.Linq;


namespace Domain
{
    public class CityGrid 
    {
        public readonly int Width;
        public readonly int Height;
        public Dictionary<GridPos, Building> Buildings = new();
        

        public CityGrid(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public int GetTickIncome()
        {
            return this.Buildings.Values.Sum(building => building.GetTickIncome());
        }
        
        public void PlaceBuilding(Building building, GridPos pos)
        {
            foreach (var buildingInDictionary in Buildings)
            {
                if (buildingInDictionary.Value == building)
                {
                    Buildings.Remove(buildingInDictionary.Key);
                    break;
                }
            }

            if (!this.Buildings.TryAdd(pos, building))
            {
                throw new Exception("Building already exists at position " + pos);
            }
        }

        public Building GetBuildingInCell(GridPos gridPosition)
        {
            return Buildings.GetValueOrDefault(gridPosition);
        }
    }
}
