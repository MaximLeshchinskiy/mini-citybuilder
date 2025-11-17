using System;
using System.Collections.Generic;
using System.Linq;
using Domain;
using UnityEngine;
using VContainer.Unity;

namespace Presentation
{
    //todo levels data validation
    [CreateAssetMenu(menuName = "Create BuildingsConfigProvider", fileName = "BuildingsConfigProvider", order = 0)]
    public class BuildingsConfigProvider : ScriptableObject, IInitializable, IBuildingViewPrefabResolver
    {
        [SerializeField] private List<BuildingTypeAndLevelData> buildingViewPrefab;

        public readonly List<BuildingType> BuildingTypes = new(); //todo dictionary
        private readonly  Dictionary<KeyValuePair<BuildingType, int>, BuildingView> _buildingViews = new();
        
        
        public BuildingView GetBuildingViewPrefab(BuildingType buildingType, int level)
        {
            return !_buildingViews.TryGetValue(new KeyValuePair<BuildingType, int>(buildingType, level),
                out var buildingView) ? throw new Exception($"Building type {buildingType} level {level} does not exist") : buildingView;
        }

        public void Initialize()
        {
            var buildingsData = new Dictionary<string, Dictionary<int, (BuildingLevel, BuildingView)>>();
            
            foreach (var buildingTypeAndLevelData in buildingViewPrefab)
            {
                if (!buildingsData.TryGetValue(buildingTypeAndLevelData.buildingName, out var buildingLevelsData))
                {
                    buildingLevelsData = new Dictionary<int, (BuildingLevel, BuildingView)>();
                    buildingsData.Add(buildingTypeAndLevelData.buildingName, buildingLevelsData);
                }

                if (!buildingLevelsData.TryAdd(buildingTypeAndLevelData.level,
                        (new BuildingLevel(buildingTypeAndLevelData.cost,
                                buildingTypeAndLevelData.incomePerTick),
                            buildingTypeAndLevelData.GetComponent<BuildingView>())))
                {
                    Debug.LogError($"Building level {buildingTypeAndLevelData.level} already exists for building {buildingTypeAndLevelData.name}");
                }
            }
            
            foreach (var buildingTypeData in buildingsData)
            {
                var levels = buildingTypeData.Value.Values.Select(it => it.Item1).ToList();
                var buildingType = new BuildingType(buildingTypeData.Key, levels);
                BuildingTypes.Add(buildingType);
                var buildingView = buildingTypeData.Value.Values.Select(it => it.Item2).ToList();
                for (var index = 0; index < levels.Count; index++)
                {
                    _buildingViews.Add(new KeyValuePair<BuildingType, int>(buildingType, index), buildingView[index]);
                }
            }
            
        }
    }
}