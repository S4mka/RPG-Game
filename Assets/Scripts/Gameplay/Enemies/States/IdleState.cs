namespace AdvancedRPG.Gameplay.Enemies.States
{
    public sealed class IdleState : IEnemyState
    {
        private readonly EnemyBrain enemy; private readonly EnemyStateMachine machine;
        public IdleState(EnemyBrain enemy, EnemyStateMachine machine) { this.enemy = enemy; this.machine = machine; }
        public void Enter() { enemy.Agent.isStopped = true; enemy.Animator?.SetFloat("Speed", 0f); }
        public void Tick()
        {
            if (enemy.HasLowHp) machine.ChangeState(new FleeState(enemy, machine));
            else if (!enemy.PeacefulMode && enemy.DistanceToPlayer <= enemy.AggroDistance) machine.ChangeState(new AggroState(enemy, machine));
        }
        public void Exit() { }
    }
}
