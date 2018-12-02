public class FriendShip : Ship {
    private readonly Prefab circleFab = new Prefab("Circle");

    protected override Prefab GetSphereFab() {
        return circleFab;
    }

    protected override int GetSphereInitCount() {
        return 1;
    }

    protected override bool IsAutoDistance() {
        return true;
    }
}