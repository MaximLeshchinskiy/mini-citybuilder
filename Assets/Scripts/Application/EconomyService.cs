using System.Threading;
using Cysharp.Threading.Tasks;
using Domain;
using Infrastructure.Gameplay;
using R3;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Application
{
    public class EconomyService : IEconomyService, IInitializable
    {
        [Inject] private GameplaySettings _gameplaySettings;
        [Inject] private GameState _gameState;

        public ReactiveProperty<int> Gold { get; } = new();
        
        public void Initialize()
        {
            Gold.Value = _gameState.Gold;
            HandleTick().Forget(Debug.LogError);
        }

        private async UniTask HandleTick(CancellationToken cancellationToken = default)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await UniTask.Delay(_gameplaySettings.tickDurationInMilliseconds, cancellationToken: cancellationToken);
                _gameState.Gold += _gameState.CityGrid.GetTickIncome();
                Gold.Value = _gameState.Gold;
            }
        }
    }
}
