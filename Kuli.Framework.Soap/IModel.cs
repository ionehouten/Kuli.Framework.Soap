using System;
using System.Windows.Forms;

namespace Kuli.Framework.Soap
{
    public interface IModel
    {
        void BeforeLoadData();
        void LoadData(Object input, BindingSource source, Control control, Boolean more = false);
        void AfterLoadData(Object output, Exception ex = null);
        void BeforeExecuteData();
        Object ExecuteData(Object input);
        void AfterExecuteData(Object output, Exception ex = null);

    }
}
