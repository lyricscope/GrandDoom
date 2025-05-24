using TMPro;
using UnityEngine;

public class RoomStateTimer : RoomState
{
    public float timerDuration = 5.0f;
    public GameObject timerUI;
    public TextMeshProUGUI timerText;
    public RoomState transitionTo;

    private float timeSpent = 0.0f;

    public override void Construct()
    {
        timeSpent = 0.0f;
        timerUI.SetActive(true);
    }

    public override void UpdateState()
    {
        timeSpent += Time.deltaTime;

        timerText.text = timeSpent.ToString("0.00") + " / " + timerDuration.ToString("0.00");

        if (timeSpent >= timerDuration)
            motor.ChangeState(transitionTo);
    }

    public override void Destruct()
    {
        timerUI.SetActive(false);
        motor.player.transform.position = Vector3.zero;
    }
}
