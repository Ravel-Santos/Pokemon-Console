namespace PokemonConsole;

public class Pokemon
{
    public string _name { get; set; }
    public int _hpMax { get; set; }
    public int _currentHp { get; set; }
    public int _attack  { get; set; }
    public string[] _sprite { get; set; }
    public Scams[] _scams { get; set; }
    
    public int TurnsBurning { get; set; } = 0;
    public int PoisonedShifts { get; set; } = 0;
    public int SeedShifts { get; set; } = 0;
    public int StalledShifts { get; set; } = 0;

    public void ApplyBurn(int duration) => TurnsBurning = duration;
    public void ApplyPoison(int duration) => PoisonedShifts = duration;
    public void ApplySeed(int duration) => SeedShifts = duration;
    public void ApplyParalysis(int duration) => StalledShifts = duration;

    public bool CanAttack()
    {
        if (StalledShifts > 0)
        {
            StalledShifts--;

            return false;
        }
        return true;
    }
    
    public Pokemon(string name = "", int hp = 0, int attack = 0, string[] sprite = null, Scams[] scams = null)
    {
        _name = name.ToUpper();
        _hpMax = hp;
        _currentHp = hp;
        _attack = attack;
        _sprite = sprite;
        _scams = scams;
    }

    public enum AttackEffect
    {
        None,     
        Drain,
        Cure,
        Paralyze,        
        Burn,
        IgnoreDefense
    }
    public class Scams
    {
        public string _name { get; set; }
        public string _description { get;  set; }
        public int _damage { get; set; }
        public AttackEffect  _effect { get; set; }
        public int _durationEffect;
        
        public Scams(string name = "", string description = "", int damage = 0, AttackEffect effect = AttackEffect.None, int durationEffect = 0)
        {
            this._name = name;
            this._description = description;
            this._damage = damage;
            this._effect = effect;
            this._durationEffect = durationEffect;
        }
    }

    public void ProcessTurnEffects()
    {
        if (TurnsBurning > 0)
        {
            int damage = 7;
            TakeDamage(damage);
            TurnsBurning--;
        }
        if (PoisonedShifts > 0)
        {
            int damage = 5;
            TakeDamage(damage);
            PoisonedShifts--; 
        }
        if (SeedShifts > 0)
        {
            int drain = 5;
            TakeDamage(drain);
            SeedShifts--;
        }
    }

    public void TakeDamage(int damage)
    {
        _currentHp = Math.Max(0, _currentHp - damage);
    }
}