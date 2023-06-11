public interface IDamageable
{
    void Damage (int damage);
}

public interface IDamage
{
   
    int Damage ();
}

public interface IInstant
{
    void Instant();
}

public interface IHeal
{
    int Heal();
}

public interface IRegainSanity
{
    int RegainSanity();
}