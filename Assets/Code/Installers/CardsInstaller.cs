using Code;
using Code.Installers;
using UnityEngine;
using Zenject;

public class CardsInstaller : MonoInstaller
{
    [SerializeField] Transform canvasTransform;
    [SerializeField] GameObject prefab;
    
    [Inject] IPlacementManager m_PlacementManager;
    
    public override void InstallBindings()
    {
        var positions = m_PlacementManager.GetPositionsOnArc(Random.Range(4, 7));
        foreach (var position in positions)
        {
            var card = Container.InstantiatePrefabForComponent<Card>(prefab, position, Quaternion.identity,
                canvasTransform);
            m_PlacementManager.CardPositionPairs.Add(card, position);
        }
    }
}
