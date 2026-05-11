namespace AdvancedRPG.Gameplay.Enemies.States
{
    public sealed class AggroState : IEnemyState
    {
        private readonly EnemyBrain enemy; private readonly EnemyStateMachine machine;
        public AggroState(EnemyBrain enemy, EnemyStateMachine machine) { this.enemy = enemy; this.machine = machine; }
        public void Enter() { enemy.Agent.isStopped = false; }
        public void Tick()
        {
            if (enemy.HasLowHp) { machine.ChangeState(new FleeState(enemy, machine)); return; }
            if (enemy.DistanceToPlayer <= enemy.AttackDistance) { machine.ChangeState(new AttackState(enemy, machine)); return; }
            enemy.Agent.SetDestination(enemy.Player.position); enemy.Animator?.SetFloat("Speed", enemy.Agent.velocity.magnitude);
        }
        public void Exit() { }
    }
}
