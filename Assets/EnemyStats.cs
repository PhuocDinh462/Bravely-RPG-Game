using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
  private Enemy enemy;

  protected override void Start()
  {
    base.Start();

    enemy = GetComponent<Enemy>();
  }

  public override void DoDamage(CharacterStats _targetStats)
  {
    base.DoDamage(_targetStats);

    enemy.DamageEffect();
  }
}
