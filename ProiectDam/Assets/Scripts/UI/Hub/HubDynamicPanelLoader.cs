using Core;
using Core.Events;
using Core.Mappers;
using Core.Values;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Utilities;

namespace UI
{
    public class HubDynamicPanelLoader : MonoBehaviour, IHubPointListener
    {
        [SerializeField] private bool _isAsync;
        [SerializeField] private PanelStack _panelStack;
        [SerializeField] private AssetReference _prefab;
        [SerializeField] private ButtonEvent _buttonEvent;
        [SerializeField] private string _name;

        private Optional<GameObject> _panel;

        public void OnEnter()
        {
            _buttonEvent.Show(new SimpleButtonModel(_name, () =>
            {
                if (_isAsync)
                {
                    _ = ShowAsync();
                }
                else
                {
                    Show();
                }
            }, this.gameObject));
        }



        public void OnExit()
        {
            _buttonEvent.Close(this.gameObject);
        }

        private void Show()
        {
            if (_panel.HasValue is false)
            {
                var handle = _prefab.InstantiateAsync();
                _panel.Value = handle.WaitForCompletion();
            }

            _panelStack.OpenDialog(_panel.Value, new PanelOptions()
            {
                PanelType = PanelType.Normal,
            });
        }

        private async Task ShowAsync()
        {
            if (_panel.HasValue)
            {
                Show();
                return;
            }

            // TODO: update with a loading handle
            var handle = _prefab.InstantiateAsync();
            _panel.Value = await handle.Task;

            Show();
        }
    }
}
