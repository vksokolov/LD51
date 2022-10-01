public class EnemySpeedModifier : GameModeModifier
{
    private readonly float _speedMultiplier;
    private readonly float _defaultValue;

    public EnemySpeedModifier(
        string name,
        string description,
        float speedMultiplier) : base(name, description)
    {
        _speedMultiplier = speedMultiplier;
        _defaultValue = Enemy.SpeedMultiplier;
    }

    public override void Apply() => Enemy.SpeedMultiplier = _speedMultiplier;

    public override void Remove() => Enemy.SpeedMultiplier = _defaultValue;
}
