using UnityEngine;
using Zenject;

namespace Code.Installers
{
    public class DependencyInstaller : MonoInstaller
    {
        [SerializeField] UICollider tableCollider;
        [SerializeField] PlacementManager PlacementManager;

        public override void InstallBindings()
        {
            Container.Bind<CardDataGenerator>().ToSelf().AsSingle();

            Container.BindInstance(tableCollider as IUICollider);
            Container.BindInstance(PlacementManager as IPlacementManager);

        }
    }
}