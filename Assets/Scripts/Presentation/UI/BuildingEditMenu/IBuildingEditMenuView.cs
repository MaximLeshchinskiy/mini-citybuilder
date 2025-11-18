using Presentation.UI.Lib;
using R3;

namespace Presentation.UI.BuildingEditMenu
{
    public interface IBuildingEditMenuView : IView
    {
        Subject<Unit> MoveButtonClicked { get; }
        Subject<Unit> UpgradeButtonClicked { get; }
        Subject<Unit> DestroyButtonClicked { get; }
        void Hide();
        void Show();
        void BindBuildingLevel(ReactiveProperty<int> level);
        void BindBuildingName(ReactiveProperty<string> buildingName);
        void BindUpgradeButton(ReactiveProperty<bool> canUpgrade);
    }
}