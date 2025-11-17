using Domain;
using UnityEngine;

namespace Presentation
{
    public interface IGridPositionProvider
    {
        GridPos? GetGridPosition(Vector3 worldPosition);
    }
}