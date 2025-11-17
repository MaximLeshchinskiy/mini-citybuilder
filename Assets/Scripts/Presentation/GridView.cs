using Application;
using Domain;
using UnityEngine;

using VContainer;

namespace Presentation
{
    public class GridView : MonoBehaviour, IGridPositionProvider
    {
        [SerializeField] private GridCellView gridCellViewPrefab;
        [SerializeField] private float gridCellSize;
        
        [Inject] private IInstantiateGridUseCase _instantiateGridUseCase;
        [Inject] private IBuildingViewFactory _buildingViewFactory;
    
        
        [Inject]
        private void OnPostInject()
        {
            for (var i = 0; i < _instantiateGridUseCase.Height; i++)
            {
                for (var j = 0; j < _instantiateGridUseCase.Width; j++)
                {
                    var cellView = Instantiate(gridCellViewPrefab, transform) ;
                    cellView.transform.localPosition = new Vector3(j * gridCellSize, 0, i * gridCellSize);
                    var building = _instantiateGridUseCase.GetBuildingAtCell(i, j);
                    if (building != null)
                    {
                        var buildingView = _buildingViewFactory.CreateBuildingView(building);
                        buildingView.transform.SetParent(cellView.transform);
                        buildingView.transform.localPosition = cellView.transform.position;
                    }
                }
            }
        }

        public GridPos? GetGridPosition(Vector3 worldPosition)
        {
            if (worldPosition.x < 0 || worldPosition.x > gridCellSize * _instantiateGridUseCase.Width || //todo can it be updated
                worldPosition.z < 0 || worldPosition.z > gridCellSize * _instantiateGridUseCase.Height)
            {
                return null;
            }
            var x = (int)(worldPosition.x / gridCellSize);
            var z = (int)(worldPosition.z / gridCellSize);
            return new GridPos(x, z);
        }
    }
}
