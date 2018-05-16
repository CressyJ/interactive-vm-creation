using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.Compute.Fluent.Models;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Core;
using Microsoft.Azure.Management.Compute.Fluent;

namespace InteractiveVMCreation
{
    public partial class Form1 : Form
    {
        //Microsoft.Azure.Management.ResourceManager.Fluent.Core.Region inputRegion;
        public Form1()
        {
            InitializeComponent();

            IEnumerator<Microsoft.Azure.Management.ResourceManager.Fluent.Core.Region> e = Microsoft.Azure.Management.ResourceManager.Fluent.Core.Region.Values.GetEnumerator();
            while (e.MoveNext())
            {
                Microsoft.Azure.Management.ResourceManager.Fluent.Core.Region r = e.Current;
                comboBox1.Items.Add(r.Name);
            }

            for (int i = 0; i <= 5; i++) comboBox2.Items.Add((KnownLinuxVirtualMachineImage)i);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var authProperties = "C:\\Users\\" + Environment.UserName + "\\azure\\auth.properties";

            IAzure azure = Azure.Authenticate(authProperties).WithDefaultSubscription();

            var linuxVM = azure.VirtualMachines.Define(textBox1.Text)
                //.WithRegion(Microsoft.Azure.Management.ResourceManager.Fluent.Core.Region.UKWest)
                //.WithRegion("UKWEST")
                .WithRegion(comboBox1.SelectedItem.ToString())
                .WithNewResourceGroup(textBox2.Text)
                .WithNewPrimaryNetwork("10.0.0.0/28")
                .WithPrimaryPrivateIPAddressDynamic()
                .WithNewPrimaryPublicIPAddress(textBox3.Text)
                //.WithPopularLinuxImage(KnownLinuxVirtualMachineImage.CentOS7_2)
                .WithPopularLinuxImage((KnownLinuxVirtualMachineImage)comboBox2.SelectedIndex)
                .WithRootUsername("james")
                .WithRootPassword("SuperSecret-123();")
                .WithSize(VirtualMachineSizeTypes.StandardA1v2);
            miniConsole.AppendText("VM Properties Set\r\n");
            try
            {
                var machine = azure.VirtualMachines.Create(linuxVM);
                miniConsole.AppendText("VM Created\r\n");
            }
            catch (Exception exc)
            {
                miniConsole.AppendText(exc.Message + "\r\n");
            }
        }
    }
}
