using System.Linq;
using Code.Installers;
using UnityEngine;
using Zenject;

namespace Code
{
    public class RandomChangeParamsButton : MonoBehaviour
    {
        [Inject] IPlacementManager m_PlacementManager;

        public void DoRandomChange()
        {
            var cards = m_PlacementManager.CardPositionPairs.Keys;
            int randomCardIndex = Random.Range(0, cards.Count);

            var randomCard = cards.ElementAt(randomCardIndex);
            int randomParamIndex = Random.Range(0, 3);
            int randomParamValue = Random.Range(-2, 9);

            switch (randomParamIndex)
            {
                case 0: randomCard.HpValue.Value = randomParamValue; break;
                case 1: randomCard.ManaValue.Value = randomParamValue; break;
                case 2: randomCard.AttackValue.Value = randomParamValue; break;
            }
        }
    }
}