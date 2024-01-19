using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Azure.Core.HttpHeader;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace StoreCoinMarket
{
    public class singletonSettings
    {
        private Settings? _settings;

        // Instance unique de la classe
        private static singletonSettings instance;

        // Un objet de verrouillage pour assurer la sécurité des threads
        private static readonly object lockObject = new object();

        // Propriété publique pour accéder à l'instance unique
        public static singletonSettings Instance
        {
            get
            {
                // Utilisation d'un double verrouillage pour assurer la sécurité des threads
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                        {
                            instance = new singletonSettings();
                        }
                    }
                }

                return instance;
            }
        }

        // Constructeur privé pour empêcher l'instanciation directe
        private singletonSettings()
        {
            // Initialisation de l'instance
            IConfigurationRoot config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();

            _settings = config.GetRequiredSection("Settings").Get<Settings>();
        }

        // Méthode publique de la classe singleton
        public Settings GetSettings() 
        {
            return _settings;
        }
    }

    /*
    
Vous pouvez utiliser cette classe singleton comme suit :

```csharp
class Program
    {
        static void Main()
        {
            // Accéder à l'instance singleton
            Singleton instance = Singleton.Instance;

            // Utiliser la méthode de l'instance singleton
            instance.SomeMethod();

            // Une autre tentative d'instanciation renverra la même instance existante
            Singleton anotherInstance = Singleton.Instance;

            // Les deux références pointent vers la même instance
            Console.WriteLine(instance == anotherInstance); // Cela affichera "True"
        }
    }
```

Cette implémentation utilise le modèle de conception du double verrouillage(`double-checked locking`) pour assurer la sécurité des threads lors de l'instanciation de la classe singleton. Cela garantit qu'une seule instance est créée, même dans un environnement multithreadé.
    */
}
