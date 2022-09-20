using Code.Installers;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;
using Text = TMPro.TextMeshProUGUI;

namespace Code
{
   public class Card : MonoBehaviour, IDragHandler, IDropHandler, IBeginDragHandler, IEndDragHandler
   {
      public struct Params
      {
         public Texture2D icon;
         public string header;
         public int mana;
         public int hp;
         public int attack;
      }

      [SerializeField] Image itemImage;
      [SerializeField] Text headerText;

      [SerializeField] GameObject dragingAnimation;

      public ChangeableIntValue HpValue;
      public ChangeableIntValue ManaValue;
      public ChangeableIntValue AttackValue;

      [Inject] IUICollider m_TargetCollider;
      [Inject] IPlacementManager m_PlacementManager;

      #region Initialisation

      [Inject]
      public async void Construct(CardDataGenerator generator)
      {
         var cardParams = await generator.GenerateCardParams();
         itemImage.sprite = Sprite.Create(cardParams.icon,
            new Rect(0, 0, cardParams.icon.width, cardParams.icon.height), Vector2.one * 0.5f);
         headerText.text = cardParams.header;

         HpValue.Value = cardParams.hp;
         ManaValue.Value = cardParams.mana;
         AttackValue.Value = cardParams.attack;
      }

      void OnEnable() => HpValue.OnValueChanged += OnHpChanged;

      void OnDisable() => HpValue.OnValueChanged -= OnHpChanged;

      #endregion

      void OnHpChanged()
      {
         if (HpValue.Value < 1)
         {
            Destroy(gameObject);
            DelayedRepositionCall();
         }
      }

      #region Card Draging

      public void OnBeginDrag(PointerEventData eventData)
      {
         dragingAnimation.SetActive(true);
         m_TargetCollider.BeginIntersectionCompare(transform as RectTransform);
      }

      public void OnDrag(PointerEventData eventData) => transform.position = Input.mousePosition;

      public void OnDrop(PointerEventData eventData)
      {
         if (!m_TargetCollider.OverlapsMesh) return;

         m_TargetCollider.EndIntersectionCompare();
         Destroy(gameObject);
         DelayedRepositionCall();
      }

      public void OnEndDrag(PointerEventData eventData)
      {
         m_TargetCollider.EndIntersectionCompare();

         transform.position = m_PlacementManager.CardPositionPairs[this];
         dragingAnimation.SetActive(false);
      }

      #endregion

      async void DelayedRepositionCall()
      {
         //wait for end frame
         await UniTask.Yield(PlayerLoopTiming.LastPostLateUpdate);
         m_PlacementManager.DoReposition();
      }

   }
}
