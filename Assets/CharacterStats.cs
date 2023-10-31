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


  public bool isIgnited;
  public bool isChilled;
  public bool isShocked;


  [SerializeField] private int currentHealth;

  protected virtual void Start()
  {
    critPower.SetDefaultValue(150);
    currentHealth = maxHealth.GetValue();
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

    isIgnited = _ignite;
    isChilled = _chill;
    isShocked = _shock;
  }

  public virtual void TakeDamage(int _damage)
  {
    currentHealth -= _damage;

    if (currentHealth < 0)
      Die();
  }

  protected virtual void Die()
  {
    // throw new NotImplementedException();
  }

  private int CheckTargetArmor(CharacterStats _targetStats, int totalDamage)
  {
    totalDamage -= _targetStats.armor.GetValue();
    totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);
    return totalDamage;
  }

  private bool TargetCanAvoidAttack(CharacterStats _targetStats)
  {
    int totalEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();

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
}
