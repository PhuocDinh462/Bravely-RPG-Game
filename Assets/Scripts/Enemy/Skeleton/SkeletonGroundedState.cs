using UnityEngine;

public class SkeletonGroundedState : EnemyState
{
  protected Enemy_Skeleton enemy;
  protected Transform player;

  public SkeletonGroundedState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBooleanName, Enemy_Skeleton _enemy) : base(_enemy, _stateMachine, _animBooleanName)
  {
    enemy = _enemy;
  }

  public override void Enter()
  {
    base.Enter();
    player = PlayerManager.instance.player.transform;
  }

  public override void Exit()
  {
    base.Exit();
  }

  public override void Update()
  {
    base.Update();

    if (enemy.isPlayerDetected() || Vector2.Distance(enemy.transform.position, player.transform.position) < 2)
      stateMachine.ChangeState(enemy.battleState);
  }
}
