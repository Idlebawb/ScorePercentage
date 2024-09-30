using ScorePercentage.HarmonyPatches;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenject;

namespace ScorePercentage.Installers
{
    internal class MenuInstaller : Installer
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<LevelStatsViewPatches>().AsSingle();
        }
    }
}
