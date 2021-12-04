namespace Gameplay
{
    public class Vase : KillableObject
    {
        private const int StartHealth = 1;

        protected override void OnDamage()
        {

        }

        protected override void OnDeath()
        {
            // maybe animate object
        }

        public override void OnDeathFinished()
        {
            Destroy(this.gameObject);
            LayerPosition.Clear();
        }

        private void Start()
        {
            InitHealth(StartHealth);
        }
    }
}
