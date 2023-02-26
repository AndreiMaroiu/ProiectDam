namespace Gameplay.Services
{
    public class RunDataService
    {
        public int Coins { get; set; }

        public RunState RunState { get; set; }
    }

    public enum RunState
    {
        Canceled,
        Won,
        Lost
    }
}
