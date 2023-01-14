using Core.Events;
using Core.Items;
using Core.Mappers;
using Core.Values;
using Gameplay.PickUps;
using UnityEngine;
using Utilities;

namespace Gameplay
{
    public class GolderChaliceBehaviour : MonoBehaviour
    {
        [SerializeField] private PickUpData[] _pickUps;
        [SerializeField] private InteractionHelper _helper;
        [SerializeField] private IntValue _scoreTarget;
        [SerializeField] private IntEvent _playerScore;

        private void Start()
        {
            _helper.OnInteractionEnter += () =>
            {
                _helper.Set(new GolderChaliceModel()
                {
                    Action = OnSacrifice,
                    MinScore = 20,
                    CurrentScore = _playerScore.Value
                });
            };
        }

        private ItemDescription OnSacrifice()
        {
            if (_helper.Controller.IsNull() || _playerScore < _scoreTarget)
            {
                return null;
            }

            _playerScore.Value -= _scoreTarget;

            PickUpData data = new WeightedRandom<PickUpData>(_pickUps, (i, p) => p.weight).Take();
            AbstractPickUp pickup = PickUpFactory.Instance.GetPickUp(data.handler.Type, Random.Range(data.range.start, data.range.end + 1));

            pickup.OnInteract(_helper.Controller);

            return data.handler;
        }

        [System.Serializable]
        private struct PickUpData
        {
            public Item handler;
            public Range<int> range;
            public int weight;
        }
    }
}
