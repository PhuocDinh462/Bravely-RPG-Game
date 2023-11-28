using UnityEngine;

public class SkeletonStunnedState : EnemyState {
  protected Enemy_Skeleton enemy;

  public SkeletonStunnedState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBooleanName, Enemy_Skeleton _enemy) : base(_enemy, _stateMachine, _animBooleanName) {
    enemy = _enemy;
  }

  public override void Enter() {
    base.Enter();

    enemy.fx.InvokeRepeating("RedColorBlink", 0, .1f);

    stateTimer = enemy.stunDuration;

    rb.velocity = new Vector2(-enemy.facingDir * enemy.stunDirection.x, enemy.stunDirection.y);
  }

  public override void Exit() {
    base.Exit();

    enemy.fx.Invoke("CancelColorChange", 0);
  }

  public override void Update() {
    base.Update();

    if (stateTimer < 0)
      stateMachine.ChangeState(enemy.idleState);
  }
}
