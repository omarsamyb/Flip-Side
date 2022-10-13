using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            Pause();
        }
    }
    public void Pause()
    {
        if (GameManager.game.isPaused)
            GameManager.game.Resume();
        else
            GameManager.game.Pause();
    }
}
