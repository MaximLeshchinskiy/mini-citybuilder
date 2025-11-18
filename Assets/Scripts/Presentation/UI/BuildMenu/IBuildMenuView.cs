using Presentation.UI.Lib;
using R3;

namespace Presentation.UI.Lib
{
}

namespace Presentation.UI.BuildMenu
{
    public interface IBuildMenuView : IView
    {
        Subject<uint> AddButton(string buildingTypeName, int level);
    }
}