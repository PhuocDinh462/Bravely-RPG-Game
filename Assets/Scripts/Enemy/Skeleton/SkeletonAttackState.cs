using UnityEngine;

public class SkeletonAttackState : EnemyState {
  private Enemy_Skeleton enemy;

  public SkeletonAttackState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBooleanName, Enemy_Skeleton _enemy) : base(_enemy, _stateMachine, _animBooleanName) {
    this.enemy = _enemy;
  }

  public override void Enter() {
    base.Enter();
  }

  public override void Exit() {
    base.Exit();

    enemy.lastTimeAttack = Time.time;
  }

  public override void Update() {
    base.Update();

    enemy.SetZeroVelocity();

    if (triggerCalled)
      stateMachine.ChangeState(enemy.battleState);
  }
}
