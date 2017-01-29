using ScannerReader.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScannerReader.Interfaces
{
    public interface IControlFactory
    {
        UserListControl CreateUserListControl();

        EditUserControl CreateEditUserControl(int? userId);

        MachineListControl CreateMachineListControl();

        MachineImportControl CreateMachineImportControl();
    }
}
