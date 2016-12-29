using System;
using System.Windows.Forms;

namespace Kuli.Framework.Soap
{
    public interface IController
    {
        void LoadData();

        void MoreData();

        Object GetRow();
        

    }
}
