using UnityEngine;

public interface EnemyBaseStats
{
    //All enemies will have these 3 stats
    public void SetBaseStats(float speed, float slowSpeed, float damage, float health, float attackingRange);

    public void SetEnemyComponents(int scorePoints, ref CanvasManager canvas);

    public void SetEnemyTarget(ref GameObject gameTarget);
}
