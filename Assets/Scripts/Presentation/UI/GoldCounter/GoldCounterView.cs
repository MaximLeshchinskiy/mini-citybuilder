using Cysharp.Threading.Tasks;
using Presentation.UI.Lib;
using R3;
using UnityEngine.UIElements;

namespace Presentation.UI.GoldCounter
{
    public class GoldCounterView : AView, IGoldCounterView
    {
        private VisualElement _container;
        protected override void AfterInit()
        {
            _container = DocumentRoot.Q<VisualElement>("Root");
        }

        public void BindGold(ReactiveProperty<int> gold)
        {
            gold.Subscribe(g => { _container.Q<Label>("gold-label").text = g.ToString(); })
                .AddTo(destroyCancellationToken);
        }
    }
}