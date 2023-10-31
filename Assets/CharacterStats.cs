using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CharacterStats : MonoBehaviour
{
  [Header("Major stats")]
  public Stat strength; // 1 point increase damage by 1 and crit.power by 1%
  public Stat agility;  // 1 point increase evasion by 1% and crit.chance by 1%
  public Stat intelligence; // 1 point increase magic damge by 1 and magic resistance by 3
  public Stat vitality; // 1 point increase health by 3 or 5 points

  [Header("Offensive stats")]
  public Stat damage;
  public Stat critChance;
  public Stat critPower; // Default value 150%

  [Header("Defensive stats")]
  public Stat maxHealth;
  public Stat armor;
  public Stat evasion;
  public Stat magicResistance;

  [Header("Magic stats")]
  public Stat fireDamage;
  public Stat iceDamage;
  public Stat lightningDamage;


  public bool isIgnited; // Does damage over time
  public bool isChilled; // Reduce armor by 20%
  public bool isShocked; // Reduce accuracy by 20%

  private float ingitedTimer;
  private float chilledTimer;
  private float shockedTimer;

  private float ignitedDamageCooldown = .3f;
  private float ignitedDamageTimer;
  private int igniteDamage;

  public int currentHealth;

  public System.Action onHealthChanged;

  protected virtual void Start()
  {
    critPower.SetDefaultValue(150);
    currentHealth = GetMaxHealthValue();
  }

  protected virtual void Update()
  {
    ingitedTimer -= Time.deltaTime;
    chilledTimer -= Time.deltaTime;
    shockedTimer -= Time.deltaTime;

    ignitedDamageTimer -= Time.deltaTime;

    if (ingitedTimer < 0)
      isIgnited = false;

    if (chilledTimer < 0)
      isChilled = false;

    if (shockedTimer < 0)
      isShocked = false;

    if (ignitedDamageTimer < 0 && isIgnited)
    {
      DecreaseHealthBy(igniteDamage);

      if (currentHealth < 0)
        Die();

      ignitedDamageTimer = ignitedDamageCooldown;
    }
  }

  public virtual void DoDamage(CharacterStats _targetStats)
  {
    if (TargetCanAvoidAttack(_targetStats)) return;

    int totalDamage = damage.GetValue() + strength.GetValue();

    if (CanCrit())
      totalDamage = CalculateCriticalDamage(totalDamage);

    totalDamage = CheckTargetArmor(_targetStats, totalDamage);
    // _targetStats.TakeDamage(totalDamage);
    DoMagicalDamage(_targetStats);
  }

  public virtual void DoMagicalDamage(CharacterStats _targetStats)
  {
    int _fireDamage = fireDamage.GetValue();
    int _iceDamage = iceDamage.GetValue();
    int _lightningDamage = lightningDamage.GetValue();

    int totalMagicalDamage = _fireDamage + _iceDamage + _lightningDamage + intelligence.GetValue();

    totalMagicalDamage = CheckTargetResistance(_targetStats, totalMagicalDamage);
    _targetStats.TakeDamage(totalMagicalDamage);

    if (Mathf.Max(_fireDamage, _iceDamage, _lightningDamage) <= 0)
      return;

    bool canApplyIgnite = _fireDamage > _iceDamage && _fireDamage > _lightningDamage;
    bool canApplyChill = _iceDamage > _fireDamage && _iceDamage > _lightningDamage;
    bool canApplyShock = _lightningDamage > _fireDamage && _lightningDamage > _iceDamage;

    while (!canApplyIgnite && !canApplyChill && !canApplyShock)
    {
      if (Random.value < .3f && _fireDamage > 0)
      {
        canApplyIgnite = true;
        _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
        return;
      }

      if (Random.value < .5f && _iceDamage > 0)
      {
        canApplyChill = true;
        _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
        return;
      }

      if (Random.value < .5f && _lightningDamage > 0)
      {
        canApplyShock = true;
        _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
        return;
      }
    }

    if (canApplyIgnite)
      _targetStats.SetupIgniteDamage(Mathf.RoundToInt(_fireDamage * .2f));

    _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
  }

  private static int CheckTargetResistance(CharacterStats _targetStats, int totalMagicalDamage)
  {
    totalMagicalDamage -= _targetStats.magicResistance.GetValue() + _targetStats.intelligence.GetValue() * 3;
    totalMagicalDamage = Mathf.Clamp(totalMagicalDamage, 0, int.MaxValue);
    return totalMagicalDamage;
  }

  public void ApplyAilments(bool _ignite, bool _chill, bool _shock)
  {
    if (isIgnited || isChilled || isShocked)
      return;

    if (_ignite)
    {
      isIgnited = _ignite;
      ingitedTimer = 2;
    }

    if (_chill)
    {
      isChilled = _chill;
      chilledTimer = 2;
    }

    if (_shock)
    {
      isShocked = _shock;
      shockedTimer = 2;
    }
  }

  public void SetupIgniteDamage(int _damage) => igniteDamage = _damage;

  public virtual void TakeDamage(int _damage)
  {
    DecreaseHealthBy(_damage);

    if (currentHealth < 0)
      Die();
  }

  protected virtual void DecreaseHealthBy(int _damage)
  {
    currentHealth -= _damage;

    if (onHealthChanged != null)
      onHealthChanged();
  }

  protected virtual void Die()
  {
    // throw new NotImplementedException();
  }

  private int CheckTargetArmor(CharacterStats _targetStats, int totalDamage)
  {
    if (_targetStats.isChilled)
      totalDamage -= Mathf.RoundToInt(_targetStats.armor.GetValue() * .8f);
    else
      totalDamage -= _targetStats.armor.GetValue();

    totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);
    return totalDamage;
  }

  private bool TargetCanAvoidAttack(CharacterStats _targetStats)
  {
    int totalEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();

    if (isShocked)
      totalEvasion += 20;

    if (UnityEngine.Random.Range(0, 100) < totalEvasion)
      return true;

    return false;
  }

  private bool CanCrit()
  {
    int totalCriticalChance = critChance.GetValue() + agility.GetValue();

    if (Random.Range(0, 100) <= totalCriticalChance) return true;

    return false;
  }

  private int CalculateCriticalDamage(int _damage)
  {
    float totalCritPower = (critPower.GetValue() + strength.GetValue()) * .01f;
    float critDamage = _damage * totalCritPower;
    return Mathf.RoundToInt(critDamage);
  }

  public int GetMaxHealthValue()
  {
    return maxHealth.GetValue() + vitality.GetValue() * 5;
  }
}
