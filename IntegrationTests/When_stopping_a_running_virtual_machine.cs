using System;
using System.Linq;
using System.Threading.Tasks;
using Linq2Azure.VirtualMachines;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IntegrationTests
{
    [TestClass]
    public class When_stopping_a_running_virtual_machine : VirtualMachineSetup
    {

        private readonly string _windowsmachine = "Win" + Guid.NewGuid().ToString().Replace("-",String.Empty).Substring(0,10);

        [TestMethod]
        public void It_should_stop_the_virtual_machine()
        {
            CreateVirtualMachine().Wait();
            
            var deployment = CloudService.Deployments.AsArray().Single(x => x.Name == _windowsmachine);
            var role = deployment.RoleList.Single();
            role.ShutdownVirtualMachine(PostShutdownAction.Stopped).Wait();

        }

        public async Task CreateVirtualMachine()
        {

            await CloudService
                .CreateVirtualMachineDeployment(_windowsmachine)
                .AddRole(_windowsmachine)
                .WithOSHardDisk(OperationSystemDiskLabel.Is(_windowsmachine))
                .WithDiskName(_windowsmachine)
                .WithOSMedia(Os.Named("03f55de797f546a1b29d1b8d66be687a__Visual-Studio-2013-Community-12.0.31101.0-AzureSDK-2.5-WS2012R2"),
                    OsDriveBlobStoredAt.LocatedAt(new Uri("https://linq2azuredev.blob.core.windows.net/vms/" + _windowsmachine + ".vhd")))
                .AddWindowsConfiguration(ComputerName.Is(_windowsmachine), Administrator.Is("CashConverters"), Password.Is("CashConverters1"))
                .WithAdditionalWindowsSettings(x =>
                {
                    x.EnableAutomaticUpdates = true;
                    x.ResetPasswordOnFirstLogin = true;
                })
                .AddNetworkConfiguration()
                .AddRemoteDesktop()
                .AddWebPort()
                .Provision();



        }

    }
}