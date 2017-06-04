using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Die : MonoBehaviour
{

    bool selected;
    public bool IsSelected
    {
        get { return selected; }
        set
        {
            selected = value;
            if (value)
            {
                PaintMovementOptions();
            }
        }
    }

    void OnMouseEnter()
    {
        PaintMovementOptions();
    }

    void OnMouseExit()
    {
        EraseMovementOptions();
    }

    RotationDescription CurrentRotation
    {
        get
        {
            return new RotationDescription(TopFaceNumeral(), UnderFaceNumeral());
        }
    }

    public void ArrangeDieFacesOnMovement(Direction moveDir)
    {
        Transform nextTopFace = FaceFromNumeral(RotationAt(CurrentRotation, moveDir).Top).transform;
        RotateNextFace(moveDir, nextTopFace);
    }

    public void DoMovement(Direction dir, float moveTime)
    {
        EraseMovementOptions();
        ArrangeDieFacesOnMovement(dir);
        Vector3 newPos;
        Vector3 rotationDir;
        switch (dir)
        {
            case Direction.East:
                newPos = transform.position + new Vector3(1f, 0f, 0f);
                rotationDir = Vector3.down;
                break;
            case Direction.West:
                newPos = transform.position + new Vector3(-1f, 0f, 0f);
                rotationDir = Vector3.up;
                break;
            case Direction.North:
                newPos = transform.position + new Vector3(0f, 1f, 0f);
                rotationDir = Vector3.right;
                break;
            default:
                newPos = transform.position + new Vector3(0f, -1f, 0f);
                rotationDir = Vector3.left;
                break;
        }
        TweenMovement(newPos, rotationDir, moveTime);
    }

    public void PrintMovementPreview()
    // deprecated
    {
        Debug.Log("Current Rotation is: " + CurrentRotation.ToString());
        var n = RotationAt(CurrentRotation, Direction.North);
        Debug.Log("Moving North will produce rotation " + n.ToString());
        n = RotationAt(CurrentRotation, Direction.East);
        Debug.Log("Moving East will produce rotation " + n.ToString());
        n = RotationAt(CurrentRotation, Direction.South);
        Debug.Log("Moving South will produce rotation " + n.ToString());
        n = RotationAt(CurrentRotation, Direction.West);
        Debug.Log("Moving West will produce rotation " + n.ToString());
    }

    void TweenMovement(Vector3 to, Vector3 rotationDir, float moveTime)
    {
        LeanTween
            .rotateAround(gameObject, rotationDir, 90, moveTime)
            .setEase(LeanTweenType.easeInCubic);
        LeanTween
            .move(gameObject, to, moveTime)
            .setEase(LeanTweenType.easeInCubic);
    }

    void RotateNextFace(Direction moveDir, Transform nextTopFace)
    {
        Vector3 upAxisForRotation;
        switch (moveDir)
        {
            case Direction.West:
                upAxisForRotation = Vector3.up;
                break;
            case Direction.East:
                upAxisForRotation = Vector3.up;
                break;
            case Direction.North:
                upAxisForRotation = Vector3.back;
                break;
            default:
                upAxisForRotation = Vector3.forward;
                break;
        }
        var upwardFacingRotation = Quaternion.LookRotation(nextTopFace.transform.forward, upAxisForRotation);
        nextTopFace.transform.rotation = upwardFacingRotation;
    }

    Transform IncomingTopFace(Direction moveDir)
    {
        Vector3 highpoint = transform.position;
        switch (moveDir)
        {
            case Direction.West:
                highpoint += Vector3.right * 10f;
                break;
            case Direction.East:
                highpoint += Vector3.left * 10f;
                break;
            case Direction.North:
                highpoint += Vector3.down * 10f;
                break;
            case Direction.South:
                highpoint += Vector3.up * 10f;
                break;
        }
        return NearestChildToPoint(highpoint);
    }

    Transform NearestChildToPoint(Vector3 highpoint)
    {
        Transform highestChild = null;
        var faces = transform.Find("Faces");
        for (int i = 0; i < faces.childCount; i++)
        {
            if (!highestChild)
            {
                highestChild = faces.GetChild(i);
            }
            else
            {
                var thisChildDistance = Vector3.Distance(
                    highpoint,
                    faces.GetChild(i).position);
                var currentHighestDistance = Vector3.Distance(
                    highpoint,
                    highestChild.transform.position);
                highestChild =
                    currentHighestDistance < thisChildDistance
                    ? highestChild
                    : faces.GetChild(i);
            }
        }
        return highestChild;
    }

    Transform TopFace()
    {
        Vector3 highpoint = transform.position + (Vector3.back * 10);
        return NearestChildToPoint(highpoint);
    }

    int TopFaceNumeral()
    {
        return TopFace().GetComponent<FaceController>().FaceNum;
    }

    int UnderFaceNumeral()
    {
        var highpoint = transform.position + (Vector3.down * 10);
        var underTrans = NearestChildToPoint(highpoint);
        return underTrans.GetComponent<FaceController>().FaceNum;
    }

    public static RotationDescription RotationAt(RotationDescription initialRotation, Direction rotationDirection)
    {
        return initialRotation.CalculateRotation(rotationDirection);
    }

    GameObject FaceFromNumeral(int faceNumeral)
    // Given a face numeral, return the Face gameobject bearing that numeral.
    {
        var faces = transform.Find("Faces");
        for (int i = 0; i < faces.childCount; i++)
        {
            var face = faces.GetChild(i).GetComponent<FaceController>();
            if (face.FaceNum == faceNumeral)
            {
                return face.gameObject;
            }
        }
        return null;
    }

    public void PaintMovementOptions()
    {
        var northRotation = RotationAt(CurrentRotation, Direction.North);
        var eastRotation = RotationAt(CurrentRotation, Direction.East);
        var southRotation = RotationAt(CurrentRotation, Direction.South);
        var westRotation = RotationAt(CurrentRotation, Direction.West);

        var paints = new List<PaintPreviewDescription>{
            // cardinal directions
            new PaintPreviewDescription(northRotation, new Vector3(0f,1f,0f)),
            new PaintPreviewDescription(eastRotation, new Vector3(1f, 0f, 0f)),
            new PaintPreviewDescription(southRotation, new Vector3(0f, -1f, 0f)),
            new PaintPreviewDescription(westRotation, new Vector3(-1f, 0f, 0f)),

            // second-order rotations
           new PaintPreviewDescription(RotationAt(northRotation, Direction.East), new Vector3(0.75f, 1.25f, 0f), false),
           new PaintPreviewDescription(RotationAt(northRotation, Direction.West), new Vector3(-0.75f, 1.25f, 0f), false),

           new PaintPreviewDescription(RotationAt(eastRotation, Direction.North), new Vector3(1.25f, 0.75f, 0f), false),
           new PaintPreviewDescription(RotationAt(eastRotation, Direction.South), new Vector3(1.25f, -0.75f, 0f), false),

           new PaintPreviewDescription(RotationAt(southRotation, Direction.East), new Vector3(0.75f, -1.25f, 0f), false),
           new PaintPreviewDescription(RotationAt(southRotation, Direction.West), new Vector3(-0.75f, -1.25f, 0f), false),

           new PaintPreviewDescription(RotationAt(westRotation, Direction.North), new Vector3(-1.25f, 0.75f, 0f), false),
           new PaintPreviewDescription(RotationAt(westRotation, Direction.South), new Vector3(-1.25f, -0.75f, 0f), false),
        };

        foreach (PaintPreviewDescription p in paints)
        {
            var topFace = p.Rotation.Top;
            var face = FaceFromNumeral(topFace);
            var go = (GameObject)Instantiate(face, transform.position + p.PaintLocation, Quaternion.identity);
            go.transform.SetParent(transform.Find("RotationGuides"));
            go.transform.localScale = go.transform.localScale * p.TransformScalar;

            var spr = go.GetComponent<SpriteRenderer>();
            spr.color -= new Color(0f, 0f, 0f, p.PaintAlpha);
        }
    }

    public void EraseMovementOptions()
    {
        var toDelete = new List<GameObject>();
        var faces = transform.Find("RotationGuides");
        for (int i = 0; i < faces.childCount; i++)
        {
            toDelete.Add(faces.GetChild(i).gameObject);
        }
        foreach (GameObject g in toDelete)
        {
            Destroy(g);
        }
    }
}
