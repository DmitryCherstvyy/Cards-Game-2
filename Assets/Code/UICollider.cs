using UnityEngine;
using UnityEngine.EventSystems;

namespace Code
{
    [RequireComponent(typeof(RectTransform))]
    public class UICollider : UIBehaviour,IUICollider
    {
        float m_DistanceDropAmount;
        Camera m_CurrentCamera;

        RectTransform m_ThisTransform;
        RectTransform m_OtherTransform;

        public bool OverlapsMesh { get; private set; } = false;

        protected override void Awake()
        {
            m_ThisTransform = GetComponent<RectTransform>();
            var rect = m_ThisTransform.rect;
            m_DistanceDropAmount = (rect.width + rect.height) / 2;
            m_CurrentCamera = Camera.current;
        }

        public void BeginIntersectionCompare(RectTransform otherTransform)
        {
            m_OtherTransform = otherTransform;
            InvokeRepeating(nameof(OnUpdate), 0, 0.1f);
        }

        void OnUpdate()
        {
            // dropping checks for large distances
            if (Vector2.Distance(m_ThisTransform.position, m_OtherTransform.position) > m_DistanceDropAmount) return;


            var worldCorners = new Vector3[4];
            m_OtherTransform.GetWorldCorners(worldCorners);

            OverlapsMesh = false;
            foreach (var worldPoint in worldCorners)
            {
                var screenPoint = RectTransformUtility.WorldToScreenPoint(m_CurrentCamera, worldPoint);
                bool contains = RectTransformUtility.RectangleContainsScreenPoint(m_ThisTransform, screenPoint);
                if (contains)
                {
                    OverlapsMesh = true;
                    break;
                }
            }
        }

        public void EndIntersectionCompare()
        {
            CancelInvoke();
            OverlapsMesh = false;
        }
    }
    public interface IUICollider
    {
        bool OverlapsMesh { get; }
        public void BeginIntersectionCompare(RectTransform otherTransform);
        public void EndIntersectionCompare();
    }
}