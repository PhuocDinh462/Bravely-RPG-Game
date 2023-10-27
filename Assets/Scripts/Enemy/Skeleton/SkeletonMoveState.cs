public class SkeletonMoveState : SkeletonGroundedState
{
  public SkeletonMoveState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBooleanName, Enemy_Skeleton _enemy) : base(_enemyBase, _stateMachine, _animBooleanName, _enemy)
  {
  }

  public override void Enter()
  {
    base.Enter();
  }

  public override void Exit()
  {
    base.Exit();
  }

  public override void Update()
  {
    base.Update();

    enemy.SetVelocity(enemy.moveSpeed * enemy.facingDir, enemy.rb.velocity.y);

    if (enemy.IsWallDetected() || !enemy.IsGroundDetected())
    {
      enemy.Flip();
      stateMachine.ChangeState(enemy.idleState);
    }
  }
}
