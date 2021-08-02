using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class TralierLinkScript : MonoBehaviour
{
   public Button button;
   void Start()
   {
      button.onClick.AddListener(new UnityAction(OpenTralier));  
   }
   void OpenTralier()
   {
      Application.OpenURL("https://www.youtube.com/watch?v=A4fjZhoDxP8");
   }
}