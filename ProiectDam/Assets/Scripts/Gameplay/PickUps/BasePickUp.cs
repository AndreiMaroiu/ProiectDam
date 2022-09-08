using Gameplay.DataSaving;
using Gameplay.Generation;
using Gameplay.Player;
using UnityEngine;

namespace Gameplay.PickUps
{
    [RequireComponent(typeof(AudioSource))]
    public class BasePickUp : TileObject, IInteractableEnter, IDataSavingObject
    {
        [SerializeField] private Item _item;

        private AudioSource _audio;
        private PickUpSaveData _saveData;

        string IDataSavingObject.ObjectName { get; set; }

        ObjectSaveData IDataSavingObject.SaveData => new PickUpSaveData()
        {
            ObjectName = ((IDataSavingObject)this).ObjectName
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
            // TODO: refactor
            float audioLength = PlaySound();
            //SpriteRenderer renderer = GetComponent<SpriteRenderer>();

            //if (renderer.IsNotNull())
            //{
            //    renderer.enabled = false;
            //}

            this.enabled = false;

            Destroy(this.gameObject, audioLength);
        }

        /// <summary>
        /// Play sound for pick up
        /// </summary>
        /// <returns>the length of the sound clip</returns>
        private float PlaySound()
        {
            // TODO: refactor
            if (!_audio || !_item.Sound)
            {
                return 0;
            }

            _audio.PlayOneShot(_item.Sound, _audio.volume + Random.Range(-0.1f, 0.1f));
            Debug.Log("playing sound");

            return _item.Sound.length;
        }

        void IDataSavingObject.LoadFromSave(ObjectSaveData data)
        {
            // if needed in future
        }
    }
}
