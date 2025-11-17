using R3;

namespace Presentation.UI.BuildMenu
{
    public interface IBuildMenuView
    {
        Subject<uint> AddButton(string text);
    }
}