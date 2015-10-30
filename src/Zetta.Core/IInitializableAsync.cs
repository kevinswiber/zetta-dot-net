using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zetta.Core {
    public interface IInitializableAsync {
        Task Initialize();
    }
}
