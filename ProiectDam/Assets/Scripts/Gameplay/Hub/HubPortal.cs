using Core.Values;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Events;
using Core.Mappers;
using Utilities;
using UnityEngine.SceneManagement;
using System;
using Core.DataSaving;
using Core;
using Core.Services;

namespace Gameplay.Hub
{
    public class HubPortal : MonoBehaviour
    {
        [SerializeField] private HubPoint _point;
        [SerializeField] private ButtonEvent _buttonEvent;
        [SerializeField] private LevelSaverHandler _saveHandler;

        // Start is called before the first frame update
        void Start()
        {
            _point.OnEnter += OnEnter;
            _point.OnLeave += OnLeave;
        }

        private void OnLeave()
        {
            _buttonEvent.Close(this.gameObject);
        }

        private void OnEnter()
        {
            _buttonEvent.Show(new SimpleButtonModel("Enter", () =>
            {
                Loader.TargetScene = Scenes.MainScene;
                SceneManager.LoadScene(Scenes.LoadingMenu);
                _saveHandler.SetForNewScene(StaticServices.Get<SaveService>().SavePath);
            }, this.gameObject));
        }
    }
}
