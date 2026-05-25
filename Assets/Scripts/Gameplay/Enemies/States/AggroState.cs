namespace AdvancedRPG.Gameplay.Enemies.States
{
    public class AggroState : IEnemyState
    {
        private readonly EnemyBrain context;

        public AggroState(EnemyBrain context)
        {
            this.context = context;
        }

        public void Enter()
        {
            if (context.Agent != null)
            {
                context.Agent.isStopped = false;
            }
        }

        public void Tick()
        {
            if (context.Target == null)
            {
                context.ChangeState(new IdleState(context));
                return;
            }

            if (context.ShouldFlee())
            {
                context.ChangeState(new FleeState(context));
                return;
            }

            float distance = context.DistanceToTarget();

            if (distance <= context.AttackDistance)
            {
                context.ChangeState(new AttackState(context));
                return;
            }

            if (context.Agent != null)
            {
                context.Agent.SetDestination(context.Target.position);
            }
        }

        public void Exit()
        {
        }
    }
}