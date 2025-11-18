using System;
using Presentation.UI.Lib;
using R3;
using UnityEngine;
using UnityEngine.UIElements;

namespace Presentation.UI.BuildMenu
{
    //todo deactivate buttons
    [RequireComponent(typeof(UIDocument))]
    public class BuildMenuView : AView, IBuildMenuView
    {
        [SerializeField] private VisualTreeAsset buttonTemplate;
        
        private VisualElement _container;
        

        protected override void AfterInit()
        {
            _container = DocumentRoot.Q<VisualElement>("build-menu-container");
        }

        public Subject<uint> AddButton(string buildingTypeName, int level)
        {
            var instance = buttonTemplate.Instantiate();

            var btn = instance.Q<Button>("BuildingButton");

            btn.text = $"{buildingTypeName} L{level+1}";
            var subject = new Subject<uint>();

            btn.clicked += () => { subject.OnNext(0); };
            _container.Add(instance);

            return subject;
        }
    }
}