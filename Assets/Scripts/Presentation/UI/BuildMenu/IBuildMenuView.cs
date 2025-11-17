using R3;

namespace Presentation.UI.BuildMenu
{
    public interface IBuildMenuView
    {
        public void Init(); 
        Subject<uint> AddButton(string text);
    }
}