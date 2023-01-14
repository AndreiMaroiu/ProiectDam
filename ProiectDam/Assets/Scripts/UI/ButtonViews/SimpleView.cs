using Core.Mappers;

namespace UI.ButtonViews
{
    public class SimpleView : IButtonView
    {
        public void Configure(IButtonModel model, ButtonData button)
        {
            SimpleButtonModel simple = model as SimpleButtonModel;

            button.Button.onClick.AddListener(simple.Action);
            button.Text.text = simple.ButtonText;
            button.Icon.gameObject.SetActive(false);
        }
    }
}
