namespace AdvancedRPG.Gameplay.Enemies.States
{
    public class IdleState : IEnemyState
    {
        private readonly EnemyBrain context;

        public IdleState(EnemyBrain context)
        {
            this.context = context;
        }

        public void Enter()
        {
            if (context.Agent != null)
            {
                context.Agent.isStopped = true;
            }
        }

        public void Tick()
        {
            if (context.Target == null)
                return;

            if (context.ShouldFlee())
            {
                context.ChangeState(new FleeState(context));
                return;
            }

            if (context.PeacefulMode)
                return;

            float distance = context.DistanceToTarget();

            if (distance <= context.AggroDistance)
            {
                context.ChangeState(new AggroState(context));
            }
        }

        public void Exit()
        {
            if (context.Agent != null)
            {
                context.Agent.isStopped = false;
            }
        }
    }
}