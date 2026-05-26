using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuInicioControlador : MonoBehaviour
{
    void Start()
    {
        Time.timeScale = 1f;
    }

    public void OnStartClick() 
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void OnExitClick() {
# if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
