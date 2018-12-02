public class EnemyShip : Ship {
    private readonly Prefab circleEnemyFab = new Prefab("CircleEnemy");

    protected override Prefab GetSphereFab() {
        return circleEnemyFab;
    }

    protected override int GetSphereInitCount() {
        return 6;
    }
    
    protected override bool IsAutoDistance() {
        return false;
    }
}