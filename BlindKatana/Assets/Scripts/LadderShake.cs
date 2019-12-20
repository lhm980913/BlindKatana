using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class LadderShake : MonoBehaviour
{
    public float duration;
    public float strength;
    public int vibrato;
    public float randomness;
    public SpriteRenderer sprite;
    public void Shake()
    {
        sprite.transform.DOShakePosition(duration, strength, vibrato, randomness, false, false);
    }
}
