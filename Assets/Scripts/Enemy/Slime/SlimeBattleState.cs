using UnityEngine;

public class SlimeBattleState : EnemyState {
  private Transform player;
  private Enemy_Slime enemy;
  private int moveDir;

  public SlimeBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBooleanName, Enemy_Slime _enemy) : base(_enemyBase, _stateMachine, _animBooleanName) {
    this.enemy = _enemy;
  }

  public override void Enter() {
    base.Enter();

    player = PlayerManager.instance.player.transform;

    if (player.GetComponent<PlayerStats>().isDead)
      stateMachine.ChangeState(enemy.moveState);
  }

  public override void Update() {
    base.Update();

    if (enemy.isPlayerDetected()) {
      stateTimer = enemy.battleTime;

      if (enemy.isPlayerDetected().distance < enemy.attackDistance) {
        if (CanAttack())
          stateMachine.ChangeState(enemy.attackState);
      }
    }
    else {
      if (stateTimer < 0 || Vector2.Distance(player.transform.position, enemy.transform.position) > 10)
        stateMachine.ChangeState(enemy.idleState);
    }

    if (player.position.x > enemy.transform.position.x)
      moveDir = 1;
    else if (player.position.x < enemy.transform.position.x)
      moveDir = -1;

    if (enemy.isPlayerDetected() && enemy.isPlayerDetected().distance < enemy.attackDistance - .1f)
      return;

    enemy.SetVelocity(enemy.moveSpeed * moveDir, rb.velocity.y);
  }

  public override void Exit() {
    base.Exit();
  }

  private bool CanAttack() {
    if (Time.time >= enemy.lastTimeAttack + enemy.attackCooldown) {
      enemy.attackCooldown = Random.Range(enemy.minAttackCooldown, enemy.maxAttackCooldown);
      enemy.lastTimeAttack = Time.time;
      return true;
    }
    return false;
  }
}

