using UnityEngine.UI;

namespace Core.Mappers
{
    public interface IButtonView
    {
        void Configure(IButtonModel model, ButtonData button);
    }
}
