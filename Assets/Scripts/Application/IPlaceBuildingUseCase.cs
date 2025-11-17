using System.Collections;
using System.Collections.Generic;
using Domain;

namespace Application
{
    public interface IPlaceBuildingUseCase
    {
        IEnumerable<BuildingType> BuildingsAvailable { get; }
    }
}