using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Events;
using static UnityEditor.Progress;

[System.Serializable]
public struct myFrame
{
    public string frameName;
    public Vector2Int startAndEndFrames;
}

public class MyStringEvent : UnityEvent<string>
{
}

[RequireComponent(typeof(SpriteRenderer)), RequireComponent(typeof(BoxCollider2D))]
public class AnimationManager : MonoBehaviour
{
    [Header("Sprite information")]

    [Tooltip("SpriteSheet that you want to splice up")]
    public Sprite spriteSheet;
    private SpriteRenderer spriteRenderer;
    
    [Header("Required frame information")]

    [Tooltip("How many frames are there on the Horizontal axis")]
    public int XFrameCount;

    [Tooltip("How many frames are there on the Vertical axis")]
    public int YFrameCount;

    [Tooltip("Total frame count")]
    public int totalFrames;

    private Sprite[] Frames;
    
    [Header("Animation variables")]

    [Tooltip("List of animations")]
    public List<myFrame> Animations;
    
    public MyStringEvent ChangeAnimEvent;

    [Tooltip("Idle animation to play first of all from the list of animations")]
    public string IdleFrameName = "Idle";

    [Tooltip("Current animation")]
    public myFrame currentAnimation;

    [Header("Frame time variables")]

    [Tooltip("How long you want each frame to last in seconds")]
    public float timePerFrame;
    
    private float currentTimePerFrame;
    
    private int currentFrame;

    private BoxCollider2D myCollider;

    // Start is called before the first frame update
    void Awake()
    {
        myCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        
        spriteRenderer.sprite = spriteSheet;

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

        foreach (myFrame frame in Animations)
        {
            if (frame.frameName == IdleFrameName)
            {
                currentAnimation = frame;
            }
        }

        currentFrame = currentAnimation.startAndEndFrames.x;
        spriteRenderer.sprite = Frames[currentFrame];

        Vector2 myBounds = spriteRenderer.sprite.bounds.size;

        myCollider.size = new Vector2(myBounds.y / transform.lossyScale.x, myBounds.y / transform.lossyScale.y);
    }

    // Update is called once per frame
    void Update()
    {
        currentTimePerFrame += Time.deltaTime;

        if (currentTimePerFrame >= timePerFrame)
        {
            currentFrame++;
            currentTimePerFrame = 0;

            if (currentFrame > currentAnimation.startAndEndFrames.y)
            {
                currentFrame = currentAnimation.startAndEndFrames.x;
            }

            spriteRenderer.sprite = Frames[currentFrame];
        }
    }

    //public function for Regestering custom event
    public void RegesterEvent()
    {
        if (ChangeAnimEvent == null)
        {
            ChangeAnimEvent = new MyStringEvent();
        }
        
        ChangeAnimEvent.AddListener(ChangeAnim);
    }

    //public function for Changing animation for custom event above
    public void ChangeAnim(string t_newAnimName)
    {
        foreach (myFrame frame in Animations)
        {
            if (frame.frameName == t_newAnimName)
            {
                currentAnimation = frame;
            }
        }
    }

    public void ExportAnims(string gameobjectName) 
    {
        AnimationList theAsset = ScriptableObject.CreateInstance<AnimationList>();

        theAsset.Animations.AddRange(this.Animations);
       
        string path = AssetDatabase.GetAssetPath(Selection.activeObject);

        if (path == "")
        {
            path = "Assets";
        }

        string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/" + gameobjectName + "_AnimationList.asset");

        AssetDatabase.CreateAsset(theAsset, assetPathAndName);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    public void LoadExportedAnims(string path)
    {
        AnimationList t = (AnimationList)AssetDatabase.LoadAssetAtPath(path, typeof(AnimationList));

        Animations.Clear();

        foreach (var item in t.Animations)
        {
            Animations.Add(item);
        }

        Debug.Log("Finished loading animations!!!!");
    }
}
