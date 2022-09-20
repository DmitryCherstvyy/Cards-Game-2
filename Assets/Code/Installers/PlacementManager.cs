using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Code.Installers
{
    public class PlacementManager : MonoBehaviour, IPlacementManager
    {
        [Serializable]
        class Settings
        {
            public float arcRadius = 182;
            public float arcAngle = 154;
            public float xScale = 1.5f;
            public float yScale = 0.75f;
        }

        [SerializeField] Transform centerTransform;
        [SerializeField] Settings _Settings;


        public Dictionary<Card, Vector3> CardPositionPairs { get; } = new Dictionary<Card, Vector3>();

        public void DoReposition()
        {
            foreach (var key in CardPositionPairs.Keys.Where(key => key == null))
            {
                CardPositionPairs.Remove(key);
                break;
            }

            int count = CardPositionPairs.Count;
            var positions = GetPositionsOnArc(centerTransform.position, Vector3.forward, _Settings, count);
            for (var i = 0; i < count; i++)
            {
                var position = positions[i];
                var key = CardPositionPairs.Keys.ElementAt(i);

                CardPositionPairs[key] = position;
                key.transform.position = position;
            }
        }


        #region Arc functions

        public Vector3[] GetPositionsOnArc(int maxSteps = 6) => GetPositionsOnArc(centerTransform.position, Vector3.forward, _Settings, maxSteps);

        Vector3[] GetPositionsOnArc(Vector3 center, Vector3 dir, Settings settings, int maxSteps = 6)
        {
            var srcAngles = GetAnglesFromDir(center, dir);
            var initialPos = center;
            var stepAngles = settings.arcAngle / maxSteps;
            var angle = srcAngles - settings.arcAngle / 2;

            var result = new Vector3[maxSteps];
            for (var i = 0; i < maxSteps; i++)
            {
                var rad = Mathf.Deg2Rad * angle;
                var posB = initialPos;

                posB += centerTransform.rotation * new Vector3(settings.arcRadius * settings.xScale * Mathf.Cos(rad),
                    settings.arcRadius * Mathf.Sin(rad) * settings.yScale, 0);

                angle += stepAngles;
                result[i] = posB;
            }

            return result;
        }

        static float GetAnglesFromDir(Vector3 position, Vector3 dir)
        {
            var forwardLimitPos = position + dir;
            var srcAngles = Mathf.Rad2Deg * Mathf.Atan2(forwardLimitPos.z - position.z, forwardLimitPos.x - position.x);
            return srcAngles;
        }

        #endregion

    }

    public interface IPlacementManager
    {
        Dictionary<Card, Vector3> CardPositionPairs { get; }
        void DoReposition();

        Vector3[] GetPositionsOnArc(int maxSteps = 6);
    }
}