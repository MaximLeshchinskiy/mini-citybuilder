using UnityEngine;
using UnityEngine.Serialization;

namespace Presentation
{
    public class GridCellView : MonoBehaviour
    {
        private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");
        [SerializeField] private Color freeColor = Color.green;
        [SerializeField] private Color occupiedColor = Color.red;
        [SerializeField] private Color normalColor = Color.white; 
        [SerializeField] private Renderer meshRenderer;
        
        public void SetNormalMode()
        {
            meshRenderer.material.SetColor(BaseColor, normalColor);

        }

        public void SetHighlightMode(bool canPlace)
        {
            meshRenderer.material.SetColor(BaseColor, canPlace ? freeColor : occupiedColor);
        }
    }
}
