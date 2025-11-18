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


        public void SetMovedMode()
        {
            buildingObject.transform.position += Vector3.up * beingMovedOffset;
        }
        
        public void SetNormalMode()
        {
            buildingObject.transform.position -= Vector3.up * beingMovedOffset;
        }
    }
    
}