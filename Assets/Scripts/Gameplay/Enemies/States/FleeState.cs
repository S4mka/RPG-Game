using UnityEngine;

namespace AdvancedRPG.Gameplay.Enemies.States
{
    public class FleeState : IEnemyState
    {
        private readonly EnemyBrain context;

        public FleeState(EnemyBrain context)
        {
            this.context = context;
        }

        public void Enter()
        {
            context.ResumeAgent();
        }

        public void Tick()
        {
            if (context.Target == null || !context.HasActiveAgent)
            {
                context.ChangeState(new IdleState(context));
                return;
            }

            Vector3 direction =
                context.transform.position - context.Target.position;

            Vector3 fleePoint =
                context.transform.position + direction.normalized * 6f;

            if (!context.TrySetDestination(fleePoint))
                context.ChangeState(new IdleState(context));
        }

        public void Exit()
        {
        }
    }
}
