using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kuli.Framework.Soap
{
    public interface IController
    {
        Task LoadData();

        Task MoreData();

        Object GetRow();
        

    }
}
