using UnityEngine;

namespace VROOM.Scripts
{
    public class GameController : MonoBehaviour
    {
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                Application.Quit();
        }
    }
}
