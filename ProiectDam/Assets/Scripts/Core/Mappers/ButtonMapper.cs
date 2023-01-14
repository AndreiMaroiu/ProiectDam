using System;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Core.Mappers
{
    public class ButtonMapper
    {
        private readonly Dictionary<Type, IButtonView> _views = new();

        public ButtonData TargetButton { get; set; }

        public void Map(Type type, IButtonView view)
        {
            _views.Add(type, view);
        }

        public void Setup(IButtonModel model)
        {
            IButtonView view = _views[model.GetType()];
            view.Configure(model, TargetButton);
        }
    }
}
