using UnityEngine;

namespace Infrastructure.Gameplay
{
    
    [CreateAssetMenu(menuName = "Create GameplaySettings", fileName = "GameplaySettings", order = 0)]
    public class GameplaySettings : ScriptableObject
    {
        public int tickDurationInMilliseconds;
        
    }
}
