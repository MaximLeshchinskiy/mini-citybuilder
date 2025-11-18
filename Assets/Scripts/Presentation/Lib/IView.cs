using Cysharp.Threading.Tasks;

namespace Presentation.UI.Lib
{
    public interface IView
    {
        UniTask Init();
    }
}