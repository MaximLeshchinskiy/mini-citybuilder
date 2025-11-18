using Application;
using Domain;
using MessagePipe;
using R3;
using UnityEngine;
using VContainer;

namespace Presentation
{
    public class BuildingView : MonoBehaviour
    {
        [SerializeField] private GameObject buildingObject;
        [SerializeField] private float beingMovedOffset;
        
        [Inject] private Building _building;
        [Inject] private IPlaceBuildingUseCase _placeBuildingUseCase;
         
        
        private readonly CompositeDisposable _compositeDisposable = new();
        private bool _isBeingMoved;

        [Inject]
        private void OnPostInject()
        {
            _placeBuildingUseCase.BuildingMoved.Subscribe(building =>
            {
                if (building == _building)
                {
                    SetMovedMode();
                }
                else if(_isBeingMoved)
                {
                    SetNormalMode();
                }
            }).AddTo(_compositeDisposable);  
        }


        private void SetMovedMode()
        {
            _isBeingMoved = true;
            buildingObject.transform.position += Vector3.up * beingMovedOffset;
        }
        
        private void SetNormalMode()
        {
            _isBeingMoved = false;
            buildingObject.transform.position -= Vector3.up * beingMovedOffset;
        }

        private void OnDestroy()
        {
            _compositeDisposable.Dispose();
        }
    }
    
}