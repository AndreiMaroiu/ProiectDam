using Core.Values;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Events;
using Core.Mappers;
using Utilities;
using UnityEngine.SceneManagement;
using System;

namespace Gameplay.Hub
{
    public class HubPortal : MonoBehaviour
    {
        [SerializeField] private HubPoint _point;
        [SerializeField] private ButtonEvent _buttonEvent;

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
            }, this.gameObject));
        }
    }
}
