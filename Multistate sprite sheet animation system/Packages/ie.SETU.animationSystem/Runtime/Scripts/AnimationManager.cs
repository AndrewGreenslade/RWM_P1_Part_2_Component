using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public struct myFrame
{
    public string frameName;
    public Vector2Int startAndEndFrames;
}

public class MyStringEvent : UnityEvent<string>
{
}


[RequireComponent(typeof(SpriteRenderer))]
public class AnimationManager : MonoBehaviour
{
    public Sprite spriteSheet;
    private SpriteRenderer spriteRenderer;
    public int XFrameCount;
    public int YFrameCount;
    public int totalFrames;
    public Sprite[] Frames;

    public List<myFrame> customFrameRanges;
    public MyStringEvent ChangeAnimEvent;

    public string IdleFrameName = "Idle";
    public myFrame currentFrameObj;

    public float timePerFrame;
    public float currentTimePerFrame;
    public int currentFrame;

    // Start is called before the first frame update
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = spriteSheet;

        //animationStates = new List<string>();
        RegesterEvent();

        List<Sprite> cut_Sprites = new List<Sprite>();

        float w = spriteRenderer.sprite.texture.width / XFrameCount;
        float h = spriteRenderer.sprite.texture.height / YFrameCount;

        for (int y = 0; y < YFrameCount; y++)
        {
            for (int x = 0; x < XFrameCount; x++)
            {
                Sprite spr = Sprite.Create(spriteRenderer.sprite.texture, new Rect(x * w, h * (YFrameCount - y - 1), w, h), new Vector2(0.5f, 0.5f));
                spr.name = "X: " + (x * w) + " Y: " + (y * h);
                cut_Sprites.Add(spr);
            }
        }

        Frames = cut_Sprites.ToArray();

        foreach (myFrame frame in customFrameRanges)
        {
            //animationStates.Add(frame.frameName);

            if (frame.frameName == IdleFrameName)
            {
                currentFrameObj = frame;
            }
        }

        currentFrame = currentFrameObj.startAndEndFrames.x;
        spriteRenderer.sprite = Frames[currentFrame];
    }

    // Update is called once per frame
    void Update()
    {
        currentTimePerFrame += Time.deltaTime;

        if (currentTimePerFrame >= timePerFrame)
        {
            currentFrame++;
            currentTimePerFrame = 0;

            if (currentFrame > currentFrameObj.startAndEndFrames.y)
            {
                currentFrame = currentFrameObj.startAndEndFrames.x;
            }

            spriteRenderer.sprite = Frames[currentFrame];
        }
    }

    public void RegesterEvent()
    {
        if (ChangeAnimEvent == null)
        {
            ChangeAnimEvent = new MyStringEvent();
        }
        
        ChangeAnimEvent.AddListener(ChangeAnim);
    }

    public void ChangeAnim(string t_newAnimName)
    {
        foreach (myFrame frame in customFrameRanges)
        {
            if (frame.frameName == t_newAnimName)
            {
                currentFrameObj = frame;
            }
        }
    }
}
