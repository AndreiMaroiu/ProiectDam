using Gameplay.Generation;
using UnityEngine;

namespace Gameplay
{
    public class Vase : KillableObject
    {
        private const int StartHealth = 1;

        [SerializeField] private TileObject _coinPickUp;

        protected override void OnDamage()
        {

        }

        protected override void OnDeath()
        {
            // play sound

            TileObject pickUp = Instantiate(_coinPickUp, transform.parent);

            pickUp.transform.position = transform.position;
            pickUp.LayerPosition = LayerPosition;
            pickUp.LayerPosition.TileType = TileType.PickUp;

            Destroy(this.gameObject);
        }

        public override void OnDeathFinished()
        {
            // here should be destroyed
        }

        private void Start()
        {
            InitHealth(StartHealth);
        }
    }
}
