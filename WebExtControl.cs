using System;
using System.ComponentModel;
using System.Windows.Forms;

using SimFeedback.extension.webctrl;

namespace SimFeedback.extension
{
    

    public partial class WebExtControl : UserControl
    {
        private bool isStarted = false;
        
        WebCtrlExtension webCtrlExt;

        public WebExtControl(WebCtrlExtension ext, SimFeedbackExtensionFacade facade)
        {
            webCtrlExt = ext;
            Globals.facade = facade;
            
            InitializeComponent();

            try
            {
                PortData.ReadFile();
                Globals.facade.LogDebug($"Got ports from PortData file - http '{PortData.Instance.httpPort}' - websvc '{PortData.Instance.webSvcPort}'");
            } catch (Exception e)
            {
                Globals.facade.LogDebug($"Couldn't read PortData file: '{e.Message}'");
            }

            HttpPortBox1.Validating += new CancelEventHandler(Number_Validating);
            HttpPortBox1.Text = PortData.Instance.httpPort.ToString();
            WebSvcPortBox1.Validating += new CancelEventHandler(Number_Validating);
            WebSvcPortBox1.Text = PortData.Instance.webSvcPort.ToString();

            SFBCtrlWebservices sfbCtrl = new SFBCtrlWebservices();
            _ = sfbCtrl.StartWebServicesAsync();
        }

        public void Start()
        {
            isStarted = true;
            webCtrlExt.SetIsRunning(true);
        }

        public void Stop()
        {
            if (!isStarted) return;
            isStarted = false;
            webCtrlExt.SetIsRunning(false);
        }


        private void ParentForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                StartStopToggle();
            }
        }
        

        public void StartStopToggle()
        {
            if (isStarted)
            {
                isStarted = false;
                Stop();
            }
            else
            {
                isStarted = true;
                Start();
            }
        }

        
        private void ParentForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isStarted)
                StartStopToggle();
        }
        

        private void OnLoad(object sender, EventArgs e)
        {
            if (this.ParentForm != null)
            {
                this.ParentForm.FormClosing += ParentForm_FormClosing;
                this.ParentForm.KeyDown += ParentForm_KeyDown;
                this.ParentForm.KeyPreview = true;
            }
        }

        
        private void GroupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void HttpPortBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Label1_Click(object sender, EventArgs e)
        {

        }

        private void Label2_Click(object sender, EventArgs e)
        {

        }


        private void Number_Validating(object sender, CancelEventArgs e)
        {
            int val;
            TextBox tb = sender as TextBox;
            if (int.TryParse(tb.Text, out val))
            {
                if (val > 1023 && val < 65536)
                {
                    if (tb.Name == "HttpPortBox1")
                    {
                        PortData.Instance.httpPort = val;
                        Globals.facade.LogDebug($"Got Http Port num '{PortData.Instance.httpPort}'");
                        PortData.WriteFile();
                        return;
                    }
                    else if (tb.Name == "WebSvcPortBox1")
                    {
                        PortData.Instance.webSvcPort = val;
                        Globals.facade.LogDebug($"Got Http Port num '{PortData.Instance.webSvcPort}'");
                        PortData.WriteFile();
                        return;
                    } else
                    {
                        Globals.facade.LogDebug($"Got invalid TextBox name '{tb.Name}'");
                    }
                }
            }
            tb.Undo();
            e.Cancel = true;
        }

        private void Label3_Click(object sender, EventArgs e)
        {

        }
    }
}
