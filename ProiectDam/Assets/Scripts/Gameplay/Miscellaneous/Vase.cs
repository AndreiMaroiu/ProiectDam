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
            Destroy(this.gameObject);
            LayerPosition.Clear();
        }

        private void Start()
        {
            InitHealth(StartHealth);
        }
    }
}
