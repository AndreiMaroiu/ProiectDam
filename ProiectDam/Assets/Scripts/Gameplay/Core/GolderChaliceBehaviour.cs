using Core.Events;
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
            _helper.Set(OnSacrifice, "Sacrifice: 20", important: true);
        }

        private void OnSacrifice()
        {
            if (_helper.Controller.IsNull() || _playerScore < _scoreTarget)
            {
                return;
            }

            _playerScore.Value -= _scoreTarget;

            PickUpData data = new WeightedRandom<PickUpData>(_pickUps, (i, p) => p.weight).Take();
            AbstractPickUp pickup = PickUpFactory.Instance.GetPickUp(data.handler.Type, Random.Range(data.range.start, data.range.end + 1));

            pickup.OnInteract(_helper.Controller);
        }

        [System.Serializable]
        private struct PickUpData
        {
            public PickUpHandler handler;
            public Range<int> range;
            public int weight;
        }
    }
}
