using R3;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer.Unity;

namespace Infrastructure
{
    public class DesktopInputService : IInputService, IInitializable, ITickable
    {
        private Camera _camera;
        private Plane _groundPlane = new(Vector3.up, Vector3.zero);


        public void Initialize()
        {
            _camera = Camera.main;
        }

        public ReactiveProperty<Vector3> MouseWorldPosition { get; } = new();
        public Subject<Unit> MouseLeftClick { get; } = new();

        public void Tick()
        {
            var mouseScreenPos = Mouse.current.position.ReadValue();
            var ray = _camera.ScreenPointToRay(mouseScreenPos);

            if (_groundPlane.Raycast(ray, out var enter))
            {
                var worldPos = ray.GetPoint(enter);
                MouseWorldPosition.Value = worldPos;
            }

            if (Mouse.current.leftButton.wasPressedThisFrame) MouseLeftClick.OnNext(Unit.Default);
        }
    }
}