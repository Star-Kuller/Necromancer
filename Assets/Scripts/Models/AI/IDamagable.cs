namespace Models.AI
{
    public interface IDamageable
    {
        public float Health { get; set; }

        public void GetDamage(float damage);
    }
}