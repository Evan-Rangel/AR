using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneController : MonoBehaviour
{
    bool lost = false;
    public void RealoadScene()
    {
        if (lost)   
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        lost = true;
    }
    
}
