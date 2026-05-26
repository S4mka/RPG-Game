using AdvancedRPG.Gameplay.Enemies.States;

namespace AdvancedRPG.Gameplay.Boss
{
    public sealed class BossAggroState : IEnemyState
    {
        private readonly BossBrain boss;
        private readonly EnemyStateMachine machine;

        public BossAggroState(BossBrain boss, EnemyStateMachine machine)
        {
            this.boss = boss;
            this.machine = machine;
        }

        public void Enter()
        {
            if (boss.Agent != null)
                boss.Agent.isStopped = false;
        }

        public void Tick()
        {
            if (boss.Player == null)
            {
                machine.ChangeState(new BossIdleState(boss, machine));
                return;
            }

            if (boss.DistanceToPlayer <= boss.AttackDistance)
            {
                machine.ChangeState(new BossAttackState(boss, machine));
                return;
            }

            if (boss.Agent != null && boss.Agent.enabled)
                boss.Agent.SetDestination(boss.Player.position);
        }

        public void Exit() { }
    }
}
