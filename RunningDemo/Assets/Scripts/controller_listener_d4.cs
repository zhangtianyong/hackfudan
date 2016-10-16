namespace VRTK.Examples
{
    using UnityEngine;
    using System.Collections;
    public class controller_listener_d4 : MonoBehaviour
    {
        private void Start()
        {
            if (GetComponent<VRTK_ControllerEvents>() == null)
            {
                Debug.LogError("VRTK_ControllerEvents_ListenerExample is required to be attached to a Controller that has the VRTK_ControllerEvents script attached to it");
                return;
            }

            //Setup controller event listeners
           
            GetComponent<VRTK_ControllerEvents>().TouchpadPressed += new ControllerInteractionEventHandler(DoTouchpadPressed_d4);
            //GetComponent<VRTK_ControllerEvents>().TouchpadReleased += new ControllerInteractionEventHandler(DoTouchpadReleased);

            //GetComponent<VRTK_ControllerEvents>().TouchpadTouchStart += new ControllerInteractionEventHandler(DoTouchpadTouchStart);
            //GetComponent<VRTK_ControllerEvents>().TouchpadTouchEnd += new ControllerInteractionEventHandler(DoTouchpadTouchEnd);

            //GetComponent<VRTK_ControllerEvents>().TouchpadAxisChanged += new ControllerInteractionEventHandler(DoTouchpadAxisChanged);

        }

        private void DoTouchpadPressed_d4(object sender, ControllerInteractionEventArgs e)
        {
            float angle = e.touchpadAngle;
            if (angle >= 0f && angle < 120f)
            {
                Debug.Log("0-120");
                UnityEngine.SceneManagement.SceneManager.LoadScene("gameScene");
                //Application.LoadLevel("gameScene");
            }
            else if (angle >= 120f && angle < 240f)
            {
                Debug.Log("120-240");
                UnityEngine.SceneManagement.SceneManager.LoadScene("gameScene");
            }
            else if (angle >= 240f)
            {
                Debug.Log("240-360");
                UnityEngine.SceneManagement.SceneManager.LoadScene("gameScene");
            }
        }
    }
}