using UnityEngine;

public class FireBallCaster : MonoBehaviour
{
    public CastBar castBar;

    private void OnEnable()
    {
        // Register to the castbar events
        castBar.OnCastStart += OnStartCast;
        castBar.OnCastComplete += OnCompleteCast;
        castBar.OnCastCanceled += OnCanceledCast;
    }
    private void OnDisable()
    {
        castBar.OnCastStart -= OnStartCast;
        castBar.OnCastComplete -= OnCompleteCast;
        castBar.OnCastCanceled -= OnCanceledCast;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            castBar.StartCast(1f, "Fire ball", CastBarAnimation.opacity, CastBarAnimation.size | CastBarAnimation.opacity);
        if (Input.GetKeyDown(KeyCode.Alpha2))
            castBar.StartCast(0.5f, "Frost bolt", CastBarAnimation.none, CastBarAnimation.opacity);
        if (Input.GetKeyDown(KeyCode.Alpha3))
            castBar.StartCast(3f, "Use item", CastBarAnimation.opacity, CastBarAnimation.opacity);
        if (Input.GetKeyDown(KeyCode.Alpha4))
            castBar.StartCast(3f, "Wait");
        if (Input.GetKeyDown(KeyCode.Alpha5))
            castBar.StartCast(5f);
        if (Input.GetKeyDown(KeyCode.Alpha6))
            castBar.StartCast(10f, "", 800);
    }
    private void OnCompleteCast()
    { 
    
    }
    private void OnStartCast()
    { 
    
    }
    private void OnCanceledCast()
    { 
    
    }
}
