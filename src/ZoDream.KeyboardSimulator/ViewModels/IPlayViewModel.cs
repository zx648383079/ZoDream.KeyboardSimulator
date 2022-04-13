using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZoDream.KeyboardSimulator.ViewModels
{
    public interface IPlayViewModel: IDisposable
    {

        public Task<bool> PlayAsync();
        public Task<bool> PauseAsync();
        public Task<bool> StopAsync();
    }


}
