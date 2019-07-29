//
// Copyright (c) 2019 Rausch IT
//
// Permission is hereby granted, free of charge, to any person obtaining a copy 
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights 
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell 
// copies of the Software, and to permit persons to whom the Software is 
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in 
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN 
// THE SOFTWARE.
//
//

using System.Windows.Forms;

namespace SimFeedback.extension.webctrl
{
    public class WebCtrlExtension : AbstractSimFeedbackExtension
    {
        private WebExtControl extControl;
        
        public WebCtrlExtension()
        {
            Name = "Web Controller Extension";
            Info = "Use any web browser to control your SFX-100 system";
            Version = "0.0.1";
            Author = "ffxf";
            NeedsOwnTab = false;
            HasControl = true;
        }

        public override void Init(SimFeedbackExtensionFacade facade, ExtensionConfig config)
        {
            base.Init(facade, config); // call this first
            Log("Initialize Web Controler Extension");

            extControl = new WebExtControl(this, facade);
        }

        public void SetIsRunning(bool status)
        {
            IsRunning = status;
        }

        public override void Start()
        {
            if (!IsRunning)
            {
                SimFeedbackFacade.Log("Starting Web Controler Extension");
                extControl.Start();
                IsRunning = true;
            }
        }

        public override void Stop()
        {
            if (IsRunning)
            {
                Log("Stopping Web Controler Extension");
                extControl.Stop();
                IsRunning = false;
            }
        }

        public override Control ExtensionControl
        {
            get { return extControl; }
        }

    }
}
