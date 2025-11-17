using R3;
using UnityEngine;

namespace Infrastructure
{
    public interface IInputService
    {
        ReactiveProperty<Vector3> MouseWorldPosition { get; }
        Subject<Unit> MouseLeftClick { get; }
    }
}