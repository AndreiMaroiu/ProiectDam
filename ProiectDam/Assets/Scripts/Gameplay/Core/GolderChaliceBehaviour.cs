
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

        private void Start()
        {
            _helper.Set(OnSacrifice, "Sacrifice text", important: true);
        }

        private void OnSacrifice()
        {
            if (_helper.Controller.IsNull())
            {
                return;
            }

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
