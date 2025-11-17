using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;
using UnityEngine.UIElements;

namespace Presentation.UI.BuildMenu
{
    [RequireComponent(typeof(UIDocument))]
    public class BuildMenuView : MonoBehaviour, IBuildMenuView
    {
        [SerializeField] private VisualTreeAsset buttonTemplate;
        [SerializeField] private UIDocument uiDocument;
        private VisualElement _container;

        public async UniTask Init()
        {
            while (!destroyCancellationToken.IsCancellationRequested && uiDocument.rootVisualElement == null)
            {
                await UniTask.Yield();
            }
            var root = uiDocument.rootVisualElement;
             _container = root.Q<VisualElement>("build-menu-container");
        }

        public Subject<uint> AddButton(string text)
        {
            var instance = buttonTemplate.Instantiate();
            
            var btn = instance.Q<Button>("BuildingButton");

            btn.text = text;
            var subject = new Subject<uint>();

            btn.clicked += () => { subject.OnNext(0); };
            _container.Add(instance);
            
            return  subject;
        }
    }
}