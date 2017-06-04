public class PlayerInputs
{
    public float x;
    public float y;

    public PlayerInputs(float x, float y)
    {
        this.x = x;
        this.y = y;
    }

    public bool SendingMoveInputThisFrame()
    {
        return x != 0 || y != 0;
    }

    public Direction MovementDirection()
    {
        Direction dir;
        if (x > 0)
        {
            dir = Direction.East;
        }
        else if (x < 0)
        {
            dir = Direction.West;
        }
        else if (y > 0)
        {
            dir = Direction.North;
        }
        else
        {
            dir = Direction.South;
        }
        return dir;
    }
}
