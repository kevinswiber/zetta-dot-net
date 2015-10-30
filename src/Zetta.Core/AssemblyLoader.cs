using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Zetta.Core {
    public class AssemblyLoader {
        public static async Task LoadFromAssembly(Assembly assembly, dynamic input) {
            var allTypes = assembly.GetExportedTypes();
            var scouts = allTypes.Where((type) => typeof(Scout).IsAssignableFrom(type));
            var apps = allTypes.Where((type) => typeof(IApp).IsAssignableFrom(type));

            var scoutLoader = ScoutLoader.Create(input);
            var appLoader = AppLoader.Create(input);

            var scoutLoaderUse = typeof(ScoutLoader).GetMethods()
                .Where((method) => {
                    return method.Name == "Use" && method.ContainsGenericParameters
                        && method.GetParameters().Count() == 0;
                }).First();

            var appLoaderUse = typeof(AppLoader).GetMethods()
                .Where((method) => {
                    return method.Name == "Use" && method.ContainsGenericParameters
                        && method.GetParameters().Count() == 0;
                }).First();

            foreach (var type in scouts) {
                var method = scoutLoaderUse.MakeGenericMethod(type);
                var result = (Task<ScoutLoader>)method.Invoke((object)scoutLoader, null);
                await result;
            }

            foreach(var type in apps) {
                var method = appLoaderUse.MakeGenericMethod(type);
                method.Invoke((object)appLoader, null);
            }
        }
    }
}
