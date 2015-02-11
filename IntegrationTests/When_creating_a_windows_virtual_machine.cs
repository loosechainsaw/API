using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IntegrationTests
{
    [TestClass]
    public class When_creating_a_windows_virtual_machine : VirtualMachineSetup
    {



        [TestMethod]
        public void It_should_create_a_new_virtual_machine()
        {
            CreateVirtualMachine().Wait();
        }

        protected override string Location()
        {
            return "Australia East";
        }

        public async Task CreateVirtualMachine()
        {

            await CloudService
                .CreateVirtualMachineDeployment("WindowsFromGeneralized")
                .AddRole("WindowsFromGeneralized")
                .WithSize("Small")
                .WithOSHardDisk("Windows1")
                .WithSourceImageName("CC2Win008R2-os-2015-02-11")
                .WithMediaLink("https://linq2azuredev.blob.core.windows.net/vms/Windows33.vhd")
                .Continue()
                .AddWindowsConfiguration()
                .ComputerName("Windows1")
                .AdminUsername("CashConverters")
                .AdminPassword("CashConverters1")
                .AddNetworkConfiguration()
                .AddRemoteDesktop()
                .AddWebPort()
                .FinalizeRoles()
                .Provision();
        }

    }
}