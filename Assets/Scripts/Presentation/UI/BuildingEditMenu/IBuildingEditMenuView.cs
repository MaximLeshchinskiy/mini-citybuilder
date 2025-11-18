using Presentation.UI.Lib;
using R3;

namespace Presentation.UI.BuildingEditMenu
{
    public interface IBuildingEditMenuView : IView
    {
        Subject<Unit> MoveButtonClicked { get; }
        Subject<Unit> UpgradeButtonClicked { get; }
        void Hide();
        void Show();
    }
}