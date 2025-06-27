using MauiIcons.Material;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthScape.MAUI
{
    public class AuthScapeMauiBuilder
    {
        public static void Build<TApp>(MauiAppBuilder builder) where TApp : class, IApplication
        {
            builder
                .UseMauiApp<TApp>()
                .UseMaterialMauiIcons();
        }
    }

}
