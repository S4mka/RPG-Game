namespace AdvancedRPG.Gameplay.Enemies.States
{
    public sealed class EnemyStateMachine
    {
        private IEnemyState current;
        public void ChangeState(IEnemyState next)
        {
            current?.Exit();
            current = next;
            current.Enter();
        }
        public void Tick() => current?.Tick();
    }
}
