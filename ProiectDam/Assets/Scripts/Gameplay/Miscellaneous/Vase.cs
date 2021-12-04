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

            Destroy(this.gameObject);
            LayerPosition.Clear();
        }

        public override void OnDeathFinished()
        {
            // here should be destroyed
        }

        private void Start()
        {
            InitHealth(StartHealth);
        }
    }
}
