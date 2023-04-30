using Gameplay.DataSaving;
using Gameplay.Generation;
using Gameplay.Player;
using UnityEngine;

namespace Gameplay.PickUps
{
    [RequireComponent(typeof(AudioSource))]
    public class BasePickUp : TileObject, IInteractableEnter, IDataSavingTile
    {
        [SerializeField] private Item _item;

        private AudioSource _audio;

        System.Guid IDataSavingTile.ObjectId { get; set; }

        ObjectSaveData IDataSavingObject<ObjectSaveData>.SaveData => new PickUpSaveData()
        {
            ObjectId = ((IDataSavingTile)this).ObjectId
        };

        private void Start()
        {
            _audio = GetComponent<AudioSource>();
        }

        public void OnInteract(PlayerController controller)
        {
            LayerPosition?.Clear();

            _item.GetPickUp().OnInteract(controller);

            UseItem();
        }

        private void UseItem()
        {
            float audioLength = PlaySound();
            enabled = false;

            Destroy(gameObject, audioLength); // destroy the gameobject after the sound finish playing, otherwise there will no sound
        }

        /// <summary>
        /// Play sound for pick up
        /// </summary>
        /// <returns>the length of the sound clip</returns>
        private float PlaySound()
        {
            if (!_audio || !_item.Sound)
            {
                return 0;
            }

            _audio.PlayOneShot(_item.Sound, _audio.volume + Random.Range(-0.1f, 0.1f));
            Debug.Log("playing sound");

            return _item.Sound.length;
        }

        void IDataSavingObject<ObjectSaveData>.LoadFromSave(ObjectSaveData data)
        {
            // if needed in future
        }
    }
}
