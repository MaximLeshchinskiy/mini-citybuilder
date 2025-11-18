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
        [Inject] private T view;
        private readonly CompositeDisposable _compositeDisposable = new();
        
        protected CompositeDisposable CompositeDisposable => _compositeDisposable;

        public void Initialize()
        {
            InitializeView().Forget(Debug.LogError);
        }
        
        private async UniTask InitializeView()
        {
            await view.Init();
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
        
        public void Dispose()
        {
            BeforeDispose();
            _compositeDisposable.Dispose();
        }
    }
}