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
            MovesLeft = MaxMoves;
            selected = value;
            if (value)
            {
                PaintMovementOptions();
            }
            FocusFaces();
        }
    }

    void Start()
    {
        MovesLeft = MaxMoves;
    }
    private int MaxMoves = 2;
    public int MovesLeft;

    void OnMouseEnter()
    {
        if (!IsSelected)
        {
            PaintMovementOptions();
        }
    }

    void OnMouseExit()
    {
        if (!IsSelected)
        {
            EraseMovementOptions(0f);
        }
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

    public bool CanMove()
    {
        return IsSelected && MovesLeft > 0;
    }

    public void DoMovement(Direction dir, float moveTime)
    {
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
        MovesLeft -= 1;
        if (MovesLeft == 0)
        {
            DeactivateMovement();
        }
    }

    void DeactivateMovement()
    {
        float deactivationTime = 1.5f;
        EraseMovementOptions(deactivationTime);
        BlurFaces();
    }

    List<Transform> GetFaces()
    {
        var faces = new List<Transform>();
        var faceChildren = transform.Find("Faces");
        for (int i = 0; i < faceChildren.childCount; i++)
        {
            faces.Add(faceChildren.GetChild(i));
        }
        return faces;
    }

    void FocusFaces()
    {
        var faces = GetFaces();
        foreach (Transform face in faces)
        {
            face.GetComponent<FaceController>().Focus();
        }
    }

    void BlurFaces()
    {
        var faces = GetFaces();
        foreach (Transform face in faces)
        {
            face.GetComponent<FaceController>().Blur();
        }
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
        var faces = GetFaces();
        foreach (Transform face in faces)
        {
            if (!highestChild)
            {
                highestChild = face;
            }
            else
            {
                var thisChildDistance = Vector3.Distance(
                    highpoint,
                    face.position);
                var currentHighestDistance = Vector3.Distance(
                    highpoint,
                    highestChild.transform.position);
                highestChild =
                    currentHighestDistance < thisChildDistance
                    ? highestChild
                    : face;
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
        var faces = GetFaces();
        foreach (Transform faceTransform in faces)
        {
            var face = faceTransform.GetComponent<FaceController>();
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
            // current square
            new PaintPreviewDescription(CurrentRotation, new Vector3(0f,0f,0f)),

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
            go.transform.SetParent(GameObject.Find("RotationGuides").transform);
            go.transform.localScale = go.transform.localScale * p.TransformScalar;

            var spr = go.GetComponent<SpriteRenderer>();
            spr.color -= new Color(0f, 0f, 0f, p.PaintAlpha);
        }
    }

    public void EraseMovementOptions(float time)
    {
        var faceChildren = GameObject.Find("RotationGuides").transform;
        for (int i = 0; i < faceChildren.childCount; i++)
        {
            var child = faceChildren.GetChild(i);
            child.GetComponent<FaceController>().DeleteGuide(time);
        }
    }
}
