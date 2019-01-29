using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeSprite : MonoBehaviour {
    [SerializeField] List<Sprite> sprites = new List<Sprite>();

    public void Randomize()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = sprites[Random.Range(0, sprites.Count)];
    }
}
