using System.Collections;
using System.Collections.Generic;
public class RotationDescription
{
    public int Top { get; }
    public int Under { get; }
    public RotationDescription(int topFace, int underneathFace)
    {
        Top = topFace;
        Under = underneathFace;
    }

    private string StartingRotationKey()
    {
        return Top.ToString() + " " + Under.ToString() + " ";
    }

    public override string ToString()
    {
        return "Top Face: " + Top.ToString() + ", Under Face: " + Under.ToString();
    }

    public RotationDescription CalculateRotation(Direction rotationDirection)
    {
        string rotationKey = StartingRotationKey();
        switch (rotationDirection)
        {
            case Direction.North:
                rotationKey += "north";
                break;
            case Direction.South:
                rotationKey += "south";
                break;
            case Direction.West:
                rotationKey += "east";
                break;
            case Direction.East:
                rotationKey += "west";
                break;
        }

        return RotationDescription.rotationTransformations[rotationKey];
    }

    static Dictionary<string, RotationDescription> rotationTransformations = new Dictionary<string, RotationDescription>
    {
		// TOP FACE: 1
        {"1 2 north", new RotationDescription(2, 6)},
        {"1 2 east", new RotationDescription(3, 2)},
        {"1 2 south", new RotationDescription(5, 1)},
        {"1 2 west", new RotationDescription(4, 2)},

        {"1 3 north", new RotationDescription(3, 6)},
        {"1 3 east", new RotationDescription(5, 3)},
        {"1 3 south", new RotationDescription(4, 1)},
        {"1 3 west", new RotationDescription(2, 3)},

        {"1 4 north", new RotationDescription(4, 6)},
        {"1 4 east", new RotationDescription(2, 4)},
        {"1 4 south", new RotationDescription(3, 1)},
        {"1 4 west", new RotationDescription(5, 4)},

        {"1 5 north", new RotationDescription(5, 6)},
        {"1 5 east", new RotationDescription(4, 5)},
        {"1 5 south", new RotationDescription(2, 1)},
        {"1 5 west", new RotationDescription(3, 5)},

		// TOP FACE: 2
        {"2 1 north", new RotationDescription(1, 5)},
        {"2 1 east", new RotationDescription(4, 1)},
        {"2 1 south", new RotationDescription(6, 2)},
        {"2 1 west", new RotationDescription(3, 1)},

        {"2 3 north", new RotationDescription(3, 5)},
        {"2 3 east", new RotationDescription(1, 3)},
        {"2 3 south", new RotationDescription(4, 2)},
        {"2 3 west", new RotationDescription(6, 3)},

        {"2 6 north", new RotationDescription(6, 5)},
        {"2 6 east", new RotationDescription(3, 6)},
        {"2 6 south", new RotationDescription(1, 2)},
        {"2 6 west", new RotationDescription(4, 6)},

        {"2 4 north", new RotationDescription(4, 5)},
        {"2 4 east", new RotationDescription(6, 4)},
        {"2 4 south", new RotationDescription(3, 2)},
        {"2 4 west", new RotationDescription(1, 4)},

		// TOP FACE: 3
        {"3 2 north", new RotationDescription(2, 4)},
        {"3 2 east", new RotationDescription(6, 2)},
        {"3 2 south", new RotationDescription(5, 3)},
        {"3 2 west", new RotationDescription(1, 2)},

        {"3 1 north", new RotationDescription(1, 4)},
        {"3 1 east", new RotationDescription(2, 1)},
        {"3 1 south", new RotationDescription(6, 3)},
        {"3 1 west", new RotationDescription(5, 1)},

        {"3 5 north", new RotationDescription(5, 4)},
        {"3 5 east", new RotationDescription(1, 5)},
        {"3 5 south", new RotationDescription(2, 3)},
        {"3 5 west", new RotationDescription(6, 5)},

        {"3 6 north", new RotationDescription(6, 4)},
        {"3 6 east", new RotationDescription(5, 6)},
        {"3 6 south", new RotationDescription(1, 3)},
        {"3 6 west", new RotationDescription(2, 6)},

		// TOP FACE: 4
        {"4 1 north", new RotationDescription(1, 3)},
        {"4 1 east", new RotationDescription(5, 1)},
        {"4 1 south", new RotationDescription(6, 4)},
        {"4 1 west", new RotationDescription(2, 1)},

        {"4 2 north", new RotationDescription(2, 3)},
        {"4 2 east", new RotationDescription(1, 2)},
        {"4 2 south", new RotationDescription(5, 4)},
        {"4 2 west", new RotationDescription(6, 2)},

        {"4 6 north", new RotationDescription(6, 3)},
        {"4 6 east", new RotationDescription(2, 6)},
        {"4 6 south", new RotationDescription(1, 4)},
        {"4 6 west", new RotationDescription(5, 6)},

        {"4 5 north", new RotationDescription(5, 3)},
        {"4 5 east", new RotationDescription(6, 5)},
        {"4 5 south", new RotationDescription(2, 4)},
        {"4 5 west", new RotationDescription(1, 5)},

		// TOP FACE: 5
        {"5 1 north", new RotationDescription(1, 2)},
        {"5 1 east", new RotationDescription(3, 1)},
        {"5 1 south", new RotationDescription(6, 5)},
        {"5 1 west", new RotationDescription(4, 1)},

        {"5 4 north", new RotationDescription(4, 2)},
        {"5 4 east", new RotationDescription(1, 4)},
        {"5 4 south", new RotationDescription(3, 5)},
        {"5 4 west", new RotationDescription(6, 4)},

        {"5 6 north", new RotationDescription(6, 2)},
        {"5 6 east", new RotationDescription(4, 6)},
        {"5 6 south", new RotationDescription(1, 5)},
        {"5 6 west", new RotationDescription(3, 6)},

        {"5 3 north", new RotationDescription(3, 2)},
        {"5 3 east", new RotationDescription(6, 3)},
        {"5 3 south", new RotationDescription(4, 5)},
        {"5 3 west", new RotationDescription(1, 3)},

		// TOP FACE: 6
        {"6 2 north", new RotationDescription(2, 1)},
        {"6 2 east", new RotationDescription(4, 2)},
        {"6 2 south", new RotationDescription(5, 6)},
        {"6 2 west", new RotationDescription(3, 2)},

        {"6 3 north", new RotationDescription(3, 1)},
        {"6 3 east", new RotationDescription(2, 3)},
        {"6 3 south", new RotationDescription(4, 6)},
        {"6 3 west", new RotationDescription(5, 3)},

        {"6 5 north", new RotationDescription(5, 1)},
        {"6 5 east", new RotationDescription(3, 5)},
        {"6 5 south", new RotationDescription(2, 6)},
        {"6 5 west", new RotationDescription(4, 5)},

        {"6 4 north", new RotationDescription(4, 1)},
        {"6 4 east", new RotationDescription(5, 4)},
        {"6 4 south", new RotationDescription(3, 6)},
        {"6 4 west", new RotationDescription(2, 4)},
    };
}
