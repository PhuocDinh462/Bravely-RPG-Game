using UnityEngine;

public class SlimeGroundedState : EnemyState {
  protected Enemy_Slime enemy;
  protected Transform player;

  public SlimeGroundedState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBooleanName, Enemy_Slime _enemy) : base(_enemyBase, _stateMachine, _animBooleanName) {
    enemy = _enemy;
  }

  public override void Enter() {
    base.Enter();
    player = PlayerManager.instance.player.transform;
  }

  public override void Exit() {
    base.Exit();
  }

  public override void Update() {
    base.Update();

    if (enemy.isPlayerDetected() || Vector2.Distance(enemy.transform.position, player.transform.position) < 2)
      stateMachine.ChangeState(enemy.battleState);
  }
}
