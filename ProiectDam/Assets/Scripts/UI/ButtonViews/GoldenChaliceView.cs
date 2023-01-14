using Core.Mappers;
using ModalWindows;

namespace UI.ButtonViews
{
    internal class GoldenChaliceView : IButtonView
    {
        public void Configure(IButtonModel model, ButtonData button)
        {
            GolderChaliceModel goldenChaliceModel = model as GolderChaliceModel;

            button.Button.onClick.AddListener(() =>
            {
                ModalWindow.ShowDialog(new ModalWindowData()
                {
                    Header = "Are you sure you want to continue?",
                    Content = $"You will lose {goldenChaliceModel.MinScore.ToString()} score points",
                    OkAction = () =>
                    {
                        Core.Items.ItemDescription item = goldenChaliceModel.Action();
                        ModalWindow.ShowDialog(new ModalWindowData()
                        {
                            Header = $"You got {item.Name}!", // TODO: update based on item brought
                            Image = item.Image,
                        });
                    },
                });
            });

            button.Button.interactable = goldenChaliceModel.CurrentScore >= goldenChaliceModel.MinScore;
            button.Text.text = $"Sacrifice {goldenChaliceModel.MinScore.ToString()}";
        }
    }
}
