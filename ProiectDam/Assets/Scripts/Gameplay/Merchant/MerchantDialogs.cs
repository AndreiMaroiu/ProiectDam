using UnityEngine;

namespace Gameplay.Merchant
{
    [CreateAssetMenu(fileName = "NewMerchantDialogs", menuName = "Scriptables/MerchantDialogs")]
    public class MerchantDialogs : ScriptableObject
    {
        [SerializeField] private string[] _neverMet;
        [SerializeField] private string[] _justKnowing;
        [SerializeField] private string[] _friend;
        [SerializeField] private string[] _goodFriend;
        [SerializeField] private string[] _bestFriends;
        [SerializeField] private string[] _closeDialog;

        public string GetRandomDialog(FriendshipLevel friendshipLevel)
        {
            string[] lines = GetDialogLines(friendshipLevel);
            return lines is not null ? lines[Random.Range(0, lines.Length)] : null;
        }

        public string GetRandomCloseDialog()
        {
            return _closeDialog[Random.Range(0, _closeDialog.Length)];
        }

        private string[] GetDialogLines(FriendshipLevel friendshipLevel) => friendshipLevel switch
        {
            FriendshipLevel.NeverMet => _neverMet,
            FriendshipLevel.JustKnowing => _justKnowing,
            FriendshipLevel.Friend => _friend,
            FriendshipLevel.GoodFriend => _goodFriend,
            FriendshipLevel.BestFriend => _bestFriends,
            _ => null,
        };
    }
}
