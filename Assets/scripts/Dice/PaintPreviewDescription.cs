using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class PaintPreviewDescription
{
    public RotationDescription Rotation;
    public Vector3 PaintLocation;
    public float TransformScalar;
    public float PaintAlpha;

    float primaryScale = 0.9f;
    float secondaryScale = 0.4f;

    float primaryAlpha = 0.5f;
    float secondaryAlpha = 0.5f;

    public PaintPreviewDescription(RotationDescription rotation, Vector3 relativePaintLocation, bool isPrimaryPaint = true)
    {
        Rotation = rotation;
        PaintLocation = relativePaintLocation;
        TransformScalar = isPrimaryPaint ? primaryScale : secondaryScale;
        PaintAlpha = isPrimaryPaint ? primaryAlpha : secondaryAlpha;
    }
}
