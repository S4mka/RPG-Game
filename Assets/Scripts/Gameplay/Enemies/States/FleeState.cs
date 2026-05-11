using UnityEngine;

namespace AdvancedRPG.Gameplay.Enemies.States
{
    public sealed class FleeState : IEnemyState
    {
        private readonly EnemyBrain enemy; private readonly EnemyStateMachine machine;
        public FleeState(EnemyBrain enemy, EnemyStateMachine machine) { this.enemy = enemy; this.machine = machine; }
        public void Enter() { enemy.Agent.isStopped = false; }
        public void Tick()
        {
            var away = (enemy.transform.position - enemy.Player.position).normalized;
            enemy.Agent.SetDestination(enemy.transform.position + away * 8f);
            enemy.Animator?.SetFloat("Speed", enemy.Agent.velocity.magnitude);
        }
        public void Exit() { }
    }
}
