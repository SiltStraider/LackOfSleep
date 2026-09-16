using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSanity : MonoBehaviour
{
   [SerializeField] private float baseLossRate = 0.01f;
   [SerializeField] private float extraLossPerActive = 0.02f;
   [SerializeField] private float maxSanity = 1;
   [SerializeField] private EnemyScreamer screamer;
   [SerializeField] private Image sanityBar;
   
   [SerializeField] private InteractObject[] interactiveObjects;
   
   private float _currentSanity = 1;
   private float _minSanity;
   
   public float CurrentSanity => _currentSanity;

   public static Action OnSanityDepleted;

   private void Start()
   {
      _currentSanity = maxSanity;
      RefreshUI();
      StartCoroutine(ReduceSanityRuntime());
   }
   
   private IEnumerator ReduceSanityRuntime()
   {
      while (_currentSanity > _minSanity)
      {
         int activeCount = GetActiveObjectCount();
         float loss = baseLossRate + activeCount * extraLossPerActive;
         
         _currentSanity -= loss * Time.deltaTime;
         _currentSanity = Mathf.Clamp(_currentSanity, _minSanity, maxSanity);

         RefreshUI(activeCount);

         if (_currentSanity <= _minSanity)
         {
            screamer.StartScream();
            OnSanityDepleted?.Invoke();
            yield break;
         }
         
         yield return null;
      }
   }

   private void RefreshUI(int activeCount = 0)
   {
      sanityBar.fillAmount = _currentSanity;
      
      float intensity = Mathf.InverseLerp(0, interactiveObjects.Length, activeCount);
      sanityBar.color = Color.Lerp(Color.green, Color.red, intensity);
   }
   
   private int GetActiveObjectCount()
   {
      int count = 0;
      foreach (var InteractiveObject in interactiveObjects)
      {
         if(InteractiveObject.IsActive) count++;
      }
      return count;
   }
}
