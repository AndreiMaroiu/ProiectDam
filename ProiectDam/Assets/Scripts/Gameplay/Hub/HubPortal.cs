using Core;
using Core.DataSaving;
using Core.Events;
using Core.Mappers;
using Core.Services;
using Core.Values;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;

namespace Gameplay.Hub
{
    public class HubPortal : MonoBehaviour, IHubPointListener
    {
        [SerializeField] private HubPoint _point;
        [SerializeField] private ButtonEvent _buttonEvent;
        [SerializeField] private LevelSaverHandler _saveHandler;

        public void OnEnter()
        {
            _buttonEvent.Show(new SimpleButtonModel("Enter", () =>
            {
                Loader.TargetScene = Scenes.MainScene;
                SceneManager.LoadScene(Scenes.LoadingMenu);
                _saveHandler.SetForNewScene(StaticServices.Get<SaveService>().SavePath);
            }, this.gameObject));
        }

        public void OnExit()
        {
            _buttonEvent.Close(this.gameObject);
        }
    }
}
