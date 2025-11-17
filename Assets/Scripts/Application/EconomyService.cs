using R3;

namespace Application
{
    public interface IEconomyService
    {
        IReadOnlyBindableReactiveProperty<int> Gold { get; }
        
    }
    
    public class EconomyService : IEconomyService
    {
        private ReactiveProperty<int> _gold;
        public IReadOnlyBindableReactiveProperty<int> Gold { get; }
    }
}
