using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimCoRetailManager_WPF.ViewModels
{
    public class MessageViewModel : Screen
    {
        public string Header { get; private set; }
        public string Message { get; private set; }

        public void Add(string header, string message)
        {
            Header = header;
            Message = message;

            NotifyOfPropertyChange(() => Header);
            NotifyOfPropertyChange(() => Message);
        }

        public async Task Close()
        {
            await TryCloseAsync();
        }
    }
}
