using UnityEngine;

public class lifeSpanManager : MonoBehaviour
{
   public float lifeSpan = 6f;
   private float _timer = 0f;
   
   private void Update()
   {
      _timer += Time.deltaTime;

      if (_timer >= lifeSpan)
      {
         _timer = 0f;
         this.gameObject.SetActive(false);
      }
   }
}
