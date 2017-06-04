using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceController : MonoBehaviour
{
    public int FaceNum;
    public string Identifier;

    Color focusColor
    {
        get
        {
            return new Color(1f, 1f, 1f, 1f);
        }
    }
    Color blurColor
    {
        get
        {
            return new Color(.1f, .1f, .1f, 1f);
        }
    }

    SpriteRenderer Sprite()
    {
        return GetComponent<SpriteRenderer>();
    }

    public void Blur()
    {
        LeanTween.value(gameObject, Sprite().color, blurColor, 0.5f);
    }

    public void Focus()
    {
        LeanTween.value(gameObject, Sprite().color, focusColor, 0.25f);
    }

    public void DeleteGuide(float time)
    {
        var zeroAlpha = Sprite().color - new Color(0f, 0f, 0f, 1f);
        LeanTween.value(gameObject, Sprite().color, zeroAlpha, time);
        Destroy(gameObject, time);
    }
}
