using DefaultNamespace.GameSystems;
using Zenject;

namespace DefaultNamespace.Installers
{
    public class MainInstaller: MonoInstaller
    {
        public override void InstallBindings()
        {

            Container.Bind<WindSystem>().FromInstance(GetComponent<WindSystem>()).AsSingle();
            
            

        }
    }
}