namespace WindowsFormsApplication1
{
    partial class ServerForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ServerForm));
            this.label3 = new System.Windows.Forms.Label();
            this.axVideoChatServer1 = new AxVideoChatServerLib.AxVideoChatServer();
            this.listBox1 = new System.Windows.Forms.ListBox();
            ((System.ComponentModel.ISupportInitialize)(this.axVideoChatServer1)).BeginInit();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(227, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "User ID  (if the user connected, it will mark *** )";
            // 
            // axVideoChatServer1
            // 
            this.axVideoChatServer1.Enabled = true;
            this.axVideoChatServer1.Location = new System.Drawing.Point(55, 73);
            this.axVideoChatServer1.Name = "axVideoChatServer1";
            this.axVideoChatServer1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axVideoChatServer1.OcxState")));
            this.axVideoChatServer1.Size = new System.Drawing.Size(194, 119);
            this.axVideoChatServer1.TabIndex = 0;
            this.axVideoChatServer1.ClientConnected += new AxVideoChatServerLib._DVideoChatServerEvents_ClientConnectedEventHandler(this.axVideoChatServer1_ClientConnected);
            this.axVideoChatServer1.ClientDisconnected += new AxVideoChatServerLib._DVideoChatServerEvents_ClientDisconnectedEventHandler(this.axVideoChatServer1_ClientDisConnected);
            this.axVideoChatServer1.AllClientDisconnected += new AxVideoChatServerLib._DVideoChatServerEvents_AllClientDisconnectedEventHandler(this.axVideoChatServer1_AllClientDisconnected);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(12, 25);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(227, 147);
            this.listBox1.TabIndex = 12;
            // 
            // ServerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(258, 184);
            this.Controls.Add(this.axVideoChatServer1);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.label3);
            this.Name = "ServerForm";
            this.Text = "Video Chat Pro Server";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_Closing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.axVideoChatServer1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private AxVideoChatServerLib.AxVideoChatServer axVideoChatServer1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListBox listBox1;
    }
}

