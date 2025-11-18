using Cysharp.Threading.Tasks;
using Presentation.UI.Lib;
using R3;
using UnityEngine.UIElements;

namespace Presentation.UI.BuildingEditMenu
{
    public class BuildingEditMenuView : AView, IBuildingEditMenuView
    {
        public Subject<Unit> MoveButtonClicked { get; } = new();
        public Subject<Unit> UpgradeButtonClicked { get; } = new();
        public Subject<Unit> DestroyButtonClicked { get; } = new();
    

        private VisualElement _container;
        private StyleEnum<DisplayStyle> _initialDisplayStyle;


        protected override void AfterInit()
        {
            _container = DocumentRoot.Q<VisualElement>("building-edit-menu");
            _initialDisplayStyle = _container.style.display;
            _container.Q<Button>("move-button").clicked += () => MoveButtonClicked.OnNext(Unit.Default);
            _container.Q<Button>("upgrade-button").clicked += () => UpgradeButtonClicked.OnNext(Unit.Default);
            _container.Q<Button>("destroy-button").clicked += () => DestroyButtonClicked.OnNext(Unit.Default);
        }

        public void Hide()
        {
            _container.style.display = DisplayStyle.None;
        }

        public void Show()
        {
            _container.style.display = _initialDisplayStyle;
        }
    }
}