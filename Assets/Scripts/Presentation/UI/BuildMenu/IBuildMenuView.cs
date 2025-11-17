using Cysharp.Threading.Tasks;
using R3;

namespace Presentation.UI.BuildMenu
{
    public interface IBuildMenuView
    {
        UniTask Init(); 
        Subject<uint> AddButton(string text);
    }
}