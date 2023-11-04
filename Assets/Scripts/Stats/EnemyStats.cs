using UnityEngine;

public class EnemyStats : CharacterStats
{
  private Enemy enemy;
  private ItemDrop myDropSystem;

  [Header("Level details")]
  [SerializeField] private int level = 1;

  [Range(0f, 1f)]
  [SerializeField] private float percentageModifier = .4f;

  protected override void Start()
  {
    ApplyLevelModifiers();

    base.Start();

    enemy = GetComponent<Enemy>();
    myDropSystem = GetComponent<ItemDrop>();
  }

  private void ApplyLevelModifiers()
  {
    modify(strength);
    modify(agility);
    modify(intelligence);
    modify(vitality);

    modify(damage);
    modify(critChance);
    modify(critPower);

    modify(maxHealth);
    modify(armor);
    modify(evasion);
    modify(magicResistance);

    modify(fireDamage);
    modify(iceDamage);
    modify(lightningDamage);
  }

  private void modify(Stat _stat)
  {
    for (int i = 1; i < level; i++)
    {
      float modifier = _stat.GetValue() * percentageModifier;

      _stat.AddModifier(Mathf.RoundToInt(modifier));
    }
  }

  public override void TakeDamage(int _damage)
  {
    base.TakeDamage(_damage);
  }

  protected override void Die()
  {
    base.Die();
    enemy.Die();

    myDropSystem.GenerateDrop();
  }
}
