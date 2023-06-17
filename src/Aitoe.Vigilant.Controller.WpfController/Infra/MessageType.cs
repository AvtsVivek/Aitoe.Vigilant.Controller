using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aitoe.Vigilant.Controller.WpfController.Infra
{
    public enum BroadCastEvents {
        //MainWindowLoaded,
        MainWindowClosing,
        CellHeightChanged,
        RowSizeChanged,
        ColumnSizeChanged,
        ConfigureViewChanged,
        StartAllCameras,
        StopAllCameras,
        ShowHideHeadersChanged,
        MultiControllerVMConfigLoaded,
        ShowLoginView,
        ShowPushbulletSettingsView,
        ShowEmailSettingsView,
        ShowDropboxSettingsView,
        ShowProcessGridView,
        ShowHelpView,
        SingleProcessWebCamChanged,
        SingleProcessIpAddressChanged,
        EnableMainMenu,
        DisableMainMenu,
        LoadGridView,
        CameraCountChanged,
        LicenseProblem
    }

    public class MessageType<T>
    {
        public BroadCastEvents Event;
        public string Message;
        public T Value;
    }
}
