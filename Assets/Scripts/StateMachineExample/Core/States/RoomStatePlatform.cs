using UnityEngine;

public class RoomStatePlatform : RoomState
{
    private Animator roomAnimator;

    private void Awake()
    {
        roomAnimator = GetComponent<Animator>();
    }

    public override void Construct()
    {
        roomAnimator.SetTrigger("On");
    }

    public override void Destruct()
    {
        roomAnimator.SetTrigger("Off");
    }
}