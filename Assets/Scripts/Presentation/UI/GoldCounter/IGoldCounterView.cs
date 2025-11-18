using Presentation.UI.Lib;
using R3;

namespace Presentation.UI.GoldCounter
{
    public interface IGoldCounterView : IView
    {
        void BindGold(ReactiveProperty<int> gold);
    }
}