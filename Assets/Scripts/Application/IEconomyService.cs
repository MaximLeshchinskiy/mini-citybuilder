using R3;

namespace Application
{
    public interface IEconomyService
    {
        ReactiveProperty<int> Gold { get; }
    }
}