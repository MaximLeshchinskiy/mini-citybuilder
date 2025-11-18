using System;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Presentation.UI.Lib
{
    public abstract class APresenter<T> : IInitializable, IDisposable where T : IView
    {
        [Inject] private T _view;

        protected T View => _view;
        protected CompositeDisposable CompositeDisposable { get; } = new();

        public void Dispose()
        {
            BeforeDispose();
            CompositeDisposable.Dispose();
        }

        public void Initialize()
        {
            InitializeView().Forget(Debug.LogError);
        }

        private async UniTask InitializeView()
        {
            await _view.Init();
            await AfterInitializedAsync();
            AfterInitialized();
        }

        protected virtual UniTask AfterInitializedAsync()
        {
            return UniTask.CompletedTask;
        }

        protected virtual void AfterInitialized()
        {
        }

        protected virtual void BeforeDispose()
        {
        }
    }
}