using Application;
using Presentation.UI.Lib;
using VContainer;

namespace Presentation.UI.GoldCounter
{
    public class GoldCounterPresenter : APresenter<GoldCounterView>
    {
        [Inject] private IEconomyService _economyService;

        protected override void AfterInitialized()
        {
            View.BindGold(_economyService.Gold);
        }
    }
}