using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneForceLoad : MonoBehaviour
{
    private void Start()
    {
        SceneManager.LoadScene("Simulation_1");
    }
}
