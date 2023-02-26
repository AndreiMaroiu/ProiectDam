using Core.Mappers;
using UI.ButtonViews;
using UnityEngine;

namespace UI
{
    [CreateAssetMenu(fileName = "New Generic Button Mapper", menuName = "Scriptables/Generic Button Mapper")]
    public sealed class GenericButtonMapper : ButtonMapper
    {
        [SerializeField] private Sprite _coinsSprite;

        private void OnEnable()
        {
            Map(typeof(SimpleButtonModel), new SimpleView());
            Map(typeof(GolderChaliceModel), new GoldenChaliceView());
            Map(typeof(WishingWellItemModel), new WishingWellItemView(_coinsSprite));
        }
    }
}
