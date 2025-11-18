using Domain;

namespace Presentation.Grid
{
    public struct GridPosSelected
    {
        public readonly GridPos GridPos;

        public GridPosSelected(GridPos gridPos)
        {
            GridPos = gridPos;
        }
    }
}