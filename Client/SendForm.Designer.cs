namespace Client
{
    partial class SendForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SendForm));
            this.btnSend = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSelect = new System.Windows.Forms.Button();
            this.lblSendTo = new System.Windows.Forms.Label();
            this.txtMsg = new System.Windows.Forms.TextBox();
            this.lblMsgText = new System.Windows.Forms.Label();
            this.cbConfirm = new System.Windows.Forms.CheckBox();
            this.listViewTo = new System.Windows.Forms.ListView();
            this.contextMenuStripListView = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemInfo = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripListView.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSend
            // 
            this.btnSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSend.Enabled = false;
            this.btnSend.Location = new System.Drawing.Point(320, 314);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(80, 23);
            this.btnSend.TabIndex = 8;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // btnClear
            // 
            this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClear.Enabled = false;
            this.btnClear.Location = new System.Drawing.Point(148, 139);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(80, 23);
            this.btnClear.TabIndex = 4;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClean_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(406, 314);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 23);
            this.btnClose.TabIndex = 9;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSelect
            // 
            this.btnSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSelect.Location = new System.Drawing.Point(12, 139);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(130, 23);
            this.btnSelect.TabIndex = 3;
            this.btnSelect.Text = "Select Recipients";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // lblSendTo
            // 
            this.lblSendTo.AutoSize = true;
            this.lblSendTo.Location = new System.Drawing.Point(9, 9);
            this.lblSendTo.Name = "lblSendTo";
            this.lblSendTo.Size = new System.Drawing.Size(47, 13);
            this.lblSendTo.TabIndex = 1;
            this.lblSendTo.Text = "Send to:";
            // 
            // txtMsg
            // 
            this.txtMsg.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMsg.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.txtMsg.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtMsg.Location = new System.Drawing.Point(12, 197);
            this.txtMsg.MaxLength = 1000;
            this.txtMsg.Multiline = true;
            this.txtMsg.Name = "txtMsg";
            this.txtMsg.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtMsg.Size = new System.Drawing.Size(474, 107);
            this.txtMsg.TabIndex = 6;
            this.txtMsg.TextChanged += new System.EventHandler(this.txtMsg_TextChanged);
            // 
            // lblMsgText
            // 
            this.lblMsgText.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.lblMsgText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblMsgText.AutoSize = true;
            this.lblMsgText.Location = new System.Drawing.Point(12, 181);
            this.lblMsgText.Name = "lblMsgText";
            this.lblMsgText.Size = new System.Drawing.Size(157, 13);
            this.lblMsgText.TabIndex = 5;
            this.lblMsgText.Text = "Message text (max 1000 chars):";
            // 
            // cbConfirm
            // 
            this.cbConfirm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbConfirm.AutoSize = true;
            this.cbConfirm.Enabled = false;
            this.cbConfirm.Location = new System.Drawing.Point(12, 318);
            this.cbConfirm.Name = "cbConfirm";
            this.cbConfirm.Size = new System.Drawing.Size(167, 17);
            this.cbConfirm.TabIndex = 7;
            this.cbConfirm.Text = "Send me delivery confirmation";
            this.cbConfirm.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbConfirm.UseVisualStyleBackColor = true;
            // 
            // listViewTo
            // 
            this.listViewTo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewTo.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.listViewTo.BackgroundImageTiled = true;
            this.listViewTo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.listViewTo.Location = new System.Drawing.Point(12, 25);
            this.listViewTo.Name = "listViewTo";
            this.listViewTo.Size = new System.Drawing.Size(474, 105);
            this.listViewTo.TabIndex = 10;
            this.listViewTo.UseCompatibleStateImageBehavior = false;
            this.listViewTo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listViewTo_KeyDown);
            this.listViewTo.MouseClick += new System.Windows.Forms.MouseEventHandler(this.listViewTo_MouseClick);
            // 
            // contextMenuStripListView
            // 
            this.contextMenuStripListView.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemInfo,
            this.toolStripMenuItemCopy});
            this.contextMenuStripListView.Name = "contextMenuStrip1";
            this.contextMenuStripListView.Size = new System.Drawing.Size(152, 48);
            // 
            // toolStripMenuItemInfo
            // 
            this.toolStripMenuItemInfo.Name = "toolStripMenuItemInfo";
            this.toolStripMenuItemInfo.Size = new System.Drawing.Size(151, 22);
            this.toolStripMenuItemInfo.Text = "Information";
            this.toolStripMenuItemInfo.Click += new System.EventHandler(this.toolStripMenuItemInfo_Click);
            // 
            // toolStripMenuItemCopy
            // 
            this.toolStripMenuItemCopy.Name = "toolStripMenuItemCopy";
            this.toolStripMenuItemCopy.Size = new System.Drawing.Size(151, 22);
            this.toolStripMenuItemCopy.Text = "Copy to buffer";
            this.toolStripMenuItemCopy.Click += new System.EventHandler(this.ToolStripMenuItemCopy_Click);
            // 
            // SendForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(498, 349);
            this.Controls.Add(this.listViewTo);
            this.Controls.Add(this.cbConfirm);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSelect);
            this.Controls.Add(this.lblSendTo);
            this.Controls.Add(this.txtMsg);
            this.Controls.Add(this.lblMsgText);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MinimizeBox = false;
            this.Name = "SendForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "RSend Client: Send message";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.SendForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SendForm_KeyDown);
            this.contextMenuStripListView.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.Label lblSendTo;
        private System.Windows.Forms.TextBox txtMsg;
        private System.Windows.Forms.Label lblMsgText;
        private System.Windows.Forms.CheckBox cbConfirm;
        private System.Windows.Forms.ListView listViewTo;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripListView;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemCopy;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemInfo;
    }
}