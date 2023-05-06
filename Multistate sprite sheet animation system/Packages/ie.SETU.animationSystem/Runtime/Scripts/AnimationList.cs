using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Animation System/Animation List")]
public class AnimationList : ScriptableObject
{
    public List<myFrame> Animations = new List<myFrame>();
}
