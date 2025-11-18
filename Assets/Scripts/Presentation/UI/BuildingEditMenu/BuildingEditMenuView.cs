using Cysharp.Threading.Tasks;
using Presentation.UI.Lib;
using R3;
using UnityEngine.UIElements;

namespace Presentation.UI.BuildingEditMenu
{
    public class BuildingEditMenuView : AView, IBuildingEditMenuView
    {
        private VisualElement _container;
        private StyleEnum<DisplayStyle> _initialDisplayStyle;
        public Subject<Unit> MoveButtonClicked { get; } = new();
        public Subject<Unit> UpgradeButtonClicked { get; } = new();
        public Subject<Unit> DestroyButtonClicked { get; } = new();

        public void Hide()
        {
            _container.style.display = DisplayStyle.None;
        }

        public void Show()
        {
            _container.style.display = _initialDisplayStyle;
        }

        public void BindBuildingLevel(ReactiveProperty<int> level)
        {
            level.Subscribe(l => { _container.Q<Label>("level-label").text = (level.Value + 1).ToString(); })
                .AddTo(destroyCancellationToken);
        }

        public void BindBuildingName(ReactiveProperty<string> buildingName)
        {
            buildingName.Subscribe(n => { _container.Q<Label>("name-label").text = n; })
                .AddTo(destroyCancellationToken);
        }

        public void BindUpgradeButton(ReactiveProperty<bool> canUpgrade)
        {
            canUpgrade.Subscribe(c => { _container.Q<Button>("upgrade-button").SetEnabled(c); })
                .AddTo(destroyCancellationToken);
        }


        protected override void AfterInit()
        {
            _container = DocumentRoot.Q<VisualElement>("building-edit-menu");
            _initialDisplayStyle = _container.style.display;
            _container.Q<Button>("move-button").clicked += () => MoveButtonClicked.OnNext(Unit.Default);
            _container.Q<Button>("upgrade-button").clicked += () => UpgradeButtonClicked.OnNext(Unit.Default);
            _container.Q<Button>("destroy-button").clicked += () => DestroyButtonClicked.OnNext(Unit.Default);
        }

        public void SetBuildingName(string buildingName)
        {
            _container.Q<Label>("building-name").text = buildingName;
        }
    }
}