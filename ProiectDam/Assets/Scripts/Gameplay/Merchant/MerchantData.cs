namespace Gameplay.Merchant
{
    [System.Serializable]
    internal class MerchantData
    {
        public FriendshipLevel FriendshipLevel { get; set; }
        public uint TotalBuys { get; set; }
        public uint CurrentBuys { get; set; }

        public void Increment()
        {
            ++TotalBuys;
            ++CurrentBuys;

            if (CurrentBuys >= 3 && FriendshipLevel < FriendshipLevel.BestFriend)
            {
                FriendshipLevel += 1;
                CurrentBuys = 0;
            }
        }
    }
}
