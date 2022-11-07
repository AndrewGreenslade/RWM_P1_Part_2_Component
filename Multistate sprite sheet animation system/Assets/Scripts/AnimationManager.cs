using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct myFrame
{
    public int startFrame;
    public int endFrame;
}

[RequireComponent(typeof(SpriteRenderer))]
public class AnimationManager : MonoBehaviour
{
    public Sprite spriteSheet;
    private SpriteRenderer spriteRenderer;
    public int XFrameCount;
    public int YFrameCount;
    public int totalFrames;
    private Sprite[] Frames;
    public myFrame WalkingFrames;
    public float timePerFrame;
    public float currentTimePerFrame;
    public int currentFrame;

    // Start is called before the first frame update
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = spriteSheet;

        List<Sprite> cut_Sprites = new List<Sprite>();

        float w = spriteRenderer.sprite.texture.width / XFrameCount;
        float h = spriteRenderer.sprite.texture.height / YFrameCount;

        for (int y = 0; y < YFrameCount; y++)
        {
            for (int x = 0; x < XFrameCount; x++)
            {
                Sprite spr = Sprite.Create(spriteRenderer.sprite.texture, new Rect(x * w, y * h, w, h), new Vector2(0, 0));
                cut_Sprites.Add(spr);
            }
        }

        Frames = cut_Sprites.ToArray();
        currentFrame = WalkingFrames.startFrame;
        spriteRenderer.sprite = Frames[currentFrame];
    }

    // Update is called once per frame
    void Update()
    {
        currentTimePerFrame += Time.deltaTime;

        if(currentTimePerFrame >= timePerFrame)
        {
            currentFrame++;
            currentTimePerFrame = 0;

            if (currentFrame > WalkingFrames.endFrame)
            {
                currentFrame = WalkingFrames.startFrame;
            }

            spriteRenderer.sprite = Frames[currentFrame];
        }
    }
}
