using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace Presentation.UI.Lib
{
    public abstract class AView : MonoBehaviour, IView
    {
        [SerializeField] private UIDocument uiDocument;
        protected VisualElement DocumentRoot { get; private set; }

        public async UniTask Init()
        {
            while (!destroyCancellationToken.IsCancellationRequested && uiDocument.rootVisualElement == null)
                await UniTask.Yield();
            DocumentRoot = uiDocument.rootVisualElement;
            AfterInit();
        }

        protected abstract void AfterInit();
    }
}