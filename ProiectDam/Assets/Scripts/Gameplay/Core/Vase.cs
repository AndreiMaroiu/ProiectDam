using Gameplay.Generation;
using UnityEngine;
using Gameplay.DataSaving;

namespace Gameplay
{
    public class Vase : KillableObject, IDataSavingTile
    {
        private const int StartHealth = 1;

        [SerializeField] private TileObject _coinPickUp;
        [SerializeField] private int _coinsSpawnChance;

        public System.Guid ObjectId { get; set; }

        public ObjectSaveData SaveData => new VaseSaveData()
        {
            ObjectId = ObjectId,
        };

        protected override void OnDamage(MonoBehaviour dealer)
        {

        }

        protected override void OnDeath()
        {
            // TODO: play sound

            if (Random.Range(0, 100) < _coinsSpawnChance)
            {
                TileObject pickUp = Instantiate(_coinPickUp, transform.parent);

                pickUp.transform.position = transform.position;
                pickUp.LayerPosition = LayerPosition;
                pickUp.LayerPosition.TileType = TileType.PickUp;
            }
            else
            {
                LayerPosition?.Clear();
            }

            Destroy(this.gameObject);
        }

        public override void OnDeathFinished()
        {
            // here should be destroyed
        }

        private void Start()
        {
            InitHealth(StartHealth);

            _priority = 2;
        }

        public void LoadFromSave(ObjectSaveData data)
        {
            
        }
    }
}
