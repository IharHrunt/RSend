namespace Client
{
    partial class SelectForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelectForm));
            this.tabPageADGroup = new System.Windows.Forms.TabPage();
            this.btnCancelADGroup = new System.Windows.Forms.Button();
            this.btnOkADGroup = new System.Windows.Forms.Button();
            this.listViewADGroup = new System.Windows.Forms.ListView();
            this.tabControlList = new System.Windows.Forms.TabControl();
            this.tabPageUser = new System.Windows.Forms.TabPage();
            this.btnClearUser = new System.Windows.Forms.Button();
            this.btnFilterUser = new System.Windows.Forms.Button();
            this.cbFilterUser = new System.Windows.Forms.ComboBox();
            this.txtFilterUser = new System.Windows.Forms.TextBox();
            this.lblFilterUser = new System.Windows.Forms.Label();
            this.cbSelectAllUser = new System.Windows.Forms.CheckBox();
            this.btnCancelUser = new System.Windows.Forms.Button();
            this.btnOkUser = new System.Windows.Forms.Button();
            this.listViewUser = new System.Windows.Forms.ListView();
            this.tabPageHost = new System.Windows.Forms.TabPage();
            this.btnClearHost = new System.Windows.Forms.Button();
            this.btnFilterHost = new System.Windows.Forms.Button();
            this.cbFilterHost = new System.Windows.Forms.ComboBox();
            this.txtFilterHost = new System.Windows.Forms.TextBox();
            this.lblFilterHost = new System.Windows.Forms.Label();
            this.cbSelectAllHost = new System.Windows.Forms.CheckBox();
            this.btnCancelHost = new System.Windows.Forms.Button();
            this.btnOkHost = new System.Windows.Forms.Button();
            this.listViewHost = new System.Windows.Forms.ListView();
            this.tabPagePrivateGroup = new System.Windows.Forms.TabPage();
            this.listViewPrivateGroup = new System.Windows.Forms.ListView();
            this.btnDelGroup = new System.Windows.Forms.Button();
            this.btnEditGroup = new System.Windows.Forms.Button();
            this.btnAddGroup = new System.Windows.Forms.Button();
            this.btnOkPrivateGroup = new System.Windows.Forms.Button();
            this.btnCancelPrivateGroup = new System.Windows.Forms.Button();
            this.contextMenuStripListView = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemInfo = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.tabPageADGroup.SuspendLayout();
            this.tabControlList.SuspendLayout();
            this.tabPageUser.SuspendLayout();
            this.tabPageHost.SuspendLayout();
            this.tabPagePrivateGroup.SuspendLayout();
            this.contextMenuStripListView.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabPageADGroup
            // 
            this.tabPageADGroup.BackColor = System.Drawing.Color.Transparent;
            this.tabPageADGroup.Controls.Add(this.btnCancelADGroup);
            this.tabPageADGroup.Controls.Add(this.btnOkADGroup);
            this.tabPageADGroup.Controls.Add(this.listViewADGroup);
            this.tabPageADGroup.Location = new System.Drawing.Point(4, 22);
            this.tabPageADGroup.Name = "tabPageADGroup";
            this.tabPageADGroup.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageADGroup.Size = new System.Drawing.Size(889, 482);
            this.tabPageADGroup.TabIndex = 2;
            this.tabPageADGroup.Text = "Domain Groups ";
            this.tabPageADGroup.UseVisualStyleBackColor = true;
            // 
            // btnCancelADGroup
            // 
            this.btnCancelADGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancelADGroup.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancelADGroup.Location = new System.Drawing.Point(799, 447);
            this.btnCancelADGroup.Name = "btnCancelADGroup";
            this.btnCancelADGroup.Size = new System.Drawing.Size(80, 23);
            this.btnCancelADGroup.TabIndex = 2;
            this.btnCancelADGroup.Text = "Cancel";
            this.btnCancelADGroup.UseVisualStyleBackColor = true;
            // 
            // btnOkADGroup
            // 
            this.btnOkADGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOkADGroup.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOkADGroup.Enabled = false;
            this.btnOkADGroup.Location = new System.Drawing.Point(713, 447);
            this.btnOkADGroup.Name = "btnOkADGroup";
            this.btnOkADGroup.Size = new System.Drawing.Size(80, 23);
            this.btnOkADGroup.TabIndex = 1;
            this.btnOkADGroup.Text = "OK";
            this.btnOkADGroup.UseVisualStyleBackColor = true;
            this.btnOkADGroup.Click += new System.EventHandler(this.btnOkADGroup_Click);
            // 
            // listViewADGroup
            // 
            this.listViewADGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewADGroup.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.listViewADGroup.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.listViewADGroup.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.listViewADGroup.Location = new System.Drawing.Point(0, 0);
            this.listViewADGroup.Name = "listViewADGroup";
            this.listViewADGroup.Size = new System.Drawing.Size(889, 433);
            this.listViewADGroup.TabIndex = 1;
            this.listViewADGroup.UseCompatibleStateImageBehavior = false;
            this.listViewADGroup.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listViewADGroup_KeyDown);
            this.listViewADGroup.MouseClick += new System.Windows.Forms.MouseEventHandler(this.listViewADGroup_MouseClick);
            // 
            // tabControlList
            // 
            this.tabControlList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControlList.Controls.Add(this.tabPageUser);
            this.tabControlList.Controls.Add(this.tabPageHost);
            this.tabControlList.Controls.Add(this.tabPageADGroup);
            this.tabControlList.Controls.Add(this.tabPagePrivateGroup);
            this.tabControlList.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tabControlList.Location = new System.Drawing.Point(-3, 1);
            this.tabControlList.Name = "tabControlList";
            this.tabControlList.SelectedIndex = 0;
            this.tabControlList.Size = new System.Drawing.Size(897, 508);
            this.tabControlList.TabIndex = 0;
            this.tabControlList.SelectedIndexChanged += new System.EventHandler(this.tabControlList_SelectedIndexChanged);
            // 
            // tabPageUser
            // 
            this.tabPageUser.BackColor = System.Drawing.Color.Transparent;
            this.tabPageUser.Controls.Add(this.btnClearUser);
            this.tabPageUser.Controls.Add(this.btnFilterUser);
            this.tabPageUser.Controls.Add(this.cbFilterUser);
            this.tabPageUser.Controls.Add(this.txtFilterUser);
            this.tabPageUser.Controls.Add(this.lblFilterUser);
            this.tabPageUser.Controls.Add(this.cbSelectAllUser);
            this.tabPageUser.Controls.Add(this.btnCancelUser);
            this.tabPageUser.Controls.Add(this.btnOkUser);
            this.tabPageUser.Controls.Add(this.listViewUser);
            this.tabPageUser.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tabPageUser.Location = new System.Drawing.Point(4, 22);
            this.tabPageUser.Name = "tabPageUser";
            this.tabPageUser.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageUser.Size = new System.Drawing.Size(889, 482);
            this.tabPageUser.TabIndex = 0;
            this.tabPageUser.Text = "User Names";
            this.tabPageUser.UseVisualStyleBackColor = true;
            // 
            // btnClearUser
            // 
            this.btnClearUser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClearUser.Location = new System.Drawing.Point(424, 447);
            this.btnClearUser.Name = "btnClearUser";
            this.btnClearUser.Size = new System.Drawing.Size(80, 23);
            this.btnClearUser.TabIndex = 5;
            this.btnClearUser.Text = "Clear";
            this.btnClearUser.UseVisualStyleBackColor = true;
            this.btnClearUser.Click += new System.EventHandler(this.btnClearUser_Click);
            // 
            // btnFilterUser
            // 
            this.btnFilterUser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnFilterUser.Location = new System.Drawing.Point(338, 447);
            this.btnFilterUser.Name = "btnFilterUser";
            this.btnFilterUser.Size = new System.Drawing.Size(80, 23);
            this.btnFilterUser.TabIndex = 4;
            this.btnFilterUser.Text = "Apply";
            this.btnFilterUser.UseVisualStyleBackColor = true;
            this.btnFilterUser.Click += new System.EventHandler(this.btnFilterUser_Click);
            // 
            // cbFilterUser
            // 
            this.cbFilterUser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbFilterUser.BackColor = System.Drawing.SystemColors.Window;
            this.cbFilterUser.FormattingEnabled = true;
            this.cbFilterUser.Items.AddRange(new object[] {
            "Start with",
            "Exist",
            "Not start with",
            "Not exist"});
            this.cbFilterUser.Location = new System.Drawing.Point(204, 449);
            this.cbFilterUser.Name = "cbFilterUser";
            this.cbFilterUser.Size = new System.Drawing.Size(128, 21);
            this.cbFilterUser.TabIndex = 3;
            this.cbFilterUser.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cbFilterUser_KeyDown);
            // 
            // txtFilterUser
            // 
            this.txtFilterUser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtFilterUser.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.txtFilterUser.Location = new System.Drawing.Point(61, 449);
            this.txtFilterUser.Name = "txtFilterUser";
            this.txtFilterUser.Size = new System.Drawing.Size(137, 20);
            this.txtFilterUser.TabIndex = 2;
            this.txtFilterUser.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtFilterUser_KeyDown);
            // 
            // lblFilterUser
            // 
            this.lblFilterUser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblFilterUser.AutoSize = true;
            this.lblFilterUser.Location = new System.Drawing.Point(13, 452);
            this.lblFilterUser.Name = "lblFilterUser";
            this.lblFilterUser.Size = new System.Drawing.Size(29, 13);
            this.lblFilterUser.TabIndex = 14;
            this.lblFilterUser.Text = "Filter";
            // 
            // cbSelectAllUser
            // 
            this.cbSelectAllUser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbSelectAllUser.AutoSize = true;
            this.cbSelectAllUser.Enabled = false;
            this.cbSelectAllUser.Location = new System.Drawing.Point(567, 452);
            this.cbSelectAllUser.Name = "cbSelectAllUser";
            this.cbSelectAllUser.Size = new System.Drawing.Size(70, 17);
            this.cbSelectAllUser.TabIndex = 6;
            this.cbSelectAllUser.Text = "Select All";
            this.cbSelectAllUser.UseVisualStyleBackColor = true;
            this.cbSelectAllUser.CheckedChanged += new System.EventHandler(this.cbSelectAllUser_CheckedChanged);
            // 
            // btnCancelUser
            // 
            this.btnCancelUser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancelUser.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancelUser.Location = new System.Drawing.Point(799, 447);
            this.btnCancelUser.Name = "btnCancelUser";
            this.btnCancelUser.Size = new System.Drawing.Size(80, 23);
            this.btnCancelUser.TabIndex = 8;
            this.btnCancelUser.Text = "Cancel";
            this.btnCancelUser.UseVisualStyleBackColor = true;
            // 
            // btnOkUser
            // 
            this.btnOkUser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOkUser.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOkUser.Enabled = false;
            this.btnOkUser.Location = new System.Drawing.Point(713, 447);
            this.btnOkUser.Name = "btnOkUser";
            this.btnOkUser.Size = new System.Drawing.Size(80, 23);
            this.btnOkUser.TabIndex = 7;
            this.btnOkUser.Text = "OK";
            this.btnOkUser.UseVisualStyleBackColor = true;
            this.btnOkUser.Click += new System.EventHandler(this.btnOkUser_Click);
            // 
            // listViewUser
            // 
            this.listViewUser.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewUser.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.listViewUser.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.listViewUser.ForeColor = System.Drawing.SystemColors.WindowText;
            this.listViewUser.Location = new System.Drawing.Point(0, 0);
            this.listViewUser.Name = "listViewUser";
            this.listViewUser.Size = new System.Drawing.Size(889, 433);
            this.listViewUser.TabIndex = 1;
            this.listViewUser.UseCompatibleStateImageBehavior = false;
            this.listViewUser.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listViewUser_KeyDown);
            this.listViewUser.MouseClick += new System.Windows.Forms.MouseEventHandler(this.listViewUser_MouseClick);
            // 
            // tabPageHost
            // 
            this.tabPageHost.BackColor = System.Drawing.Color.Transparent;
            this.tabPageHost.Controls.Add(this.btnClearHost);
            this.tabPageHost.Controls.Add(this.btnFilterHost);
            this.tabPageHost.Controls.Add(this.cbFilterHost);
            this.tabPageHost.Controls.Add(this.txtFilterHost);
            this.tabPageHost.Controls.Add(this.lblFilterHost);
            this.tabPageHost.Controls.Add(this.cbSelectAllHost);
            this.tabPageHost.Controls.Add(this.btnCancelHost);
            this.tabPageHost.Controls.Add(this.btnOkHost);
            this.tabPageHost.Controls.Add(this.listViewHost);
            this.tabPageHost.Location = new System.Drawing.Point(4, 22);
            this.tabPageHost.Name = "tabPageHost";
            this.tabPageHost.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageHost.Size = new System.Drawing.Size(889, 482);
            this.tabPageHost.TabIndex = 1;
            this.tabPageHost.Text = "Computer Names";
            this.tabPageHost.UseVisualStyleBackColor = true;
            // 
            // btnClearHost
            // 
            this.btnClearHost.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClearHost.Location = new System.Drawing.Point(424, 447);
            this.btnClearHost.Name = "btnClearHost";
            this.btnClearHost.Size = new System.Drawing.Size(80, 23);
            this.btnClearHost.TabIndex = 5;
            this.btnClearHost.Text = "Clear";
            this.btnClearHost.UseVisualStyleBackColor = true;
            this.btnClearHost.Click += new System.EventHandler(this.btnClearHost_Click);
            // 
            // btnFilterHost
            // 
            this.btnFilterHost.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnFilterHost.Location = new System.Drawing.Point(338, 447);
            this.btnFilterHost.Name = "btnFilterHost";
            this.btnFilterHost.Size = new System.Drawing.Size(80, 23);
            this.btnFilterHost.TabIndex = 4;
            this.btnFilterHost.Text = "Apply";
            this.btnFilterHost.UseVisualStyleBackColor = true;
            this.btnFilterHost.Click += new System.EventHandler(this.btnFilterHost_Click);
            // 
            // cbFilterHost
            // 
            this.cbFilterHost.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbFilterHost.FormattingEnabled = true;
            this.cbFilterHost.Items.AddRange(new object[] {
            "Start with",
            "Exist",
            "Not start with",
            "Not exist"});
            this.cbFilterHost.Location = new System.Drawing.Point(204, 449);
            this.cbFilterHost.Name = "cbFilterHost";
            this.cbFilterHost.Size = new System.Drawing.Size(128, 21);
            this.cbFilterHost.TabIndex = 3;
            this.cbFilterHost.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cbFilterHost_KeyDown);
            // 
            // txtFilterHost
            // 
            this.txtFilterHost.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtFilterHost.Location = new System.Drawing.Point(61, 449);
            this.txtFilterHost.Name = "txtFilterHost";
            this.txtFilterHost.Size = new System.Drawing.Size(137, 20);
            this.txtFilterHost.TabIndex = 2;
            this.txtFilterHost.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtFilterHost_KeyDown);
            // 
            // lblFilterHost
            // 
            this.lblFilterHost.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblFilterHost.AutoSize = true;
            this.lblFilterHost.Location = new System.Drawing.Point(13, 452);
            this.lblFilterHost.Name = "lblFilterHost";
            this.lblFilterHost.Size = new System.Drawing.Size(29, 13);
            this.lblFilterHost.TabIndex = 21;
            this.lblFilterHost.Text = "Filter";
            // 
            // cbSelectAllHost
            // 
            this.cbSelectAllHost.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbSelectAllHost.AutoSize = true;
            this.cbSelectAllHost.Enabled = false;
            this.cbSelectAllHost.Location = new System.Drawing.Point(567, 452);
            this.cbSelectAllHost.Name = "cbSelectAllHost";
            this.cbSelectAllHost.Size = new System.Drawing.Size(70, 17);
            this.cbSelectAllHost.TabIndex = 6;
            this.cbSelectAllHost.Text = "Select All";
            this.cbSelectAllHost.UseVisualStyleBackColor = true;
            this.cbSelectAllHost.CheckedChanged += new System.EventHandler(this.cbSelectAllHost_CheckedChanged);
            // 
            // btnCancelHost
            // 
            this.btnCancelHost.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancelHost.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancelHost.Location = new System.Drawing.Point(799, 447);
            this.btnCancelHost.Name = "btnCancelHost";
            this.btnCancelHost.Size = new System.Drawing.Size(80, 23);
            this.btnCancelHost.TabIndex = 8;
            this.btnCancelHost.Text = "Cancel";
            this.btnCancelHost.UseVisualStyleBackColor = true;
            // 
            // btnOkHost
            // 
            this.btnOkHost.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOkHost.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOkHost.Enabled = false;
            this.btnOkHost.Location = new System.Drawing.Point(713, 447);
            this.btnOkHost.Name = "btnOkHost";
            this.btnOkHost.Size = new System.Drawing.Size(80, 23);
            this.btnOkHost.TabIndex = 7;
            this.btnOkHost.Text = "OK";
            this.btnOkHost.UseVisualStyleBackColor = true;
            this.btnOkHost.Click += new System.EventHandler(this.btnOkHost_Click);
            // 
            // listViewHost
            // 
            this.listViewHost.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewHost.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.listViewHost.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.listViewHost.Location = new System.Drawing.Point(0, 0);
            this.listViewHost.Name = "listViewHost";
            this.listViewHost.Size = new System.Drawing.Size(889, 433);
            this.listViewHost.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listViewHost.TabIndex = 1;
            this.listViewHost.UseCompatibleStateImageBehavior = false;
            this.listViewHost.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listViewHost_KeyDown);
            this.listViewHost.MouseClick += new System.Windows.Forms.MouseEventHandler(this.listViewHost_MouseClick);
            // 
            // tabPagePrivateGroup
            // 
            this.tabPagePrivateGroup.Controls.Add(this.listViewPrivateGroup);
            this.tabPagePrivateGroup.Controls.Add(this.btnDelGroup);
            this.tabPagePrivateGroup.Controls.Add(this.btnEditGroup);
            this.tabPagePrivateGroup.Controls.Add(this.btnAddGroup);
            this.tabPagePrivateGroup.Controls.Add(this.btnOkPrivateGroup);
            this.tabPagePrivateGroup.Controls.Add(this.btnCancelPrivateGroup);
            this.tabPagePrivateGroup.Location = new System.Drawing.Point(4, 22);
            this.tabPagePrivateGroup.Name = "tabPagePrivateGroup";
            this.tabPagePrivateGroup.Size = new System.Drawing.Size(889, 482);
            this.tabPagePrivateGroup.TabIndex = 3;
            this.tabPagePrivateGroup.Text = "Private Groups";
            this.tabPagePrivateGroup.UseVisualStyleBackColor = true;
            // 
            // listViewPrivateGroup
            // 
            this.listViewPrivateGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewPrivateGroup.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.listViewPrivateGroup.Location = new System.Drawing.Point(0, 0);
            this.listViewPrivateGroup.Name = "listViewPrivateGroup";
            this.listViewPrivateGroup.Size = new System.Drawing.Size(889, 433);
            this.listViewPrivateGroup.TabIndex = 1;
            this.listViewPrivateGroup.UseCompatibleStateImageBehavior = false;
            this.listViewPrivateGroup.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listViewPrivateGroup_KeyDown);
            this.listViewPrivateGroup.MouseClick += new System.Windows.Forms.MouseEventHandler(this.listViewPrivateGroup_MouseClick);
            // 
            // btnDelGroup
            // 
            this.btnDelGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDelGroup.Enabled = false;
            this.btnDelGroup.Location = new System.Drawing.Point(183, 447);
            this.btnDelGroup.Name = "btnDelGroup";
            this.btnDelGroup.Size = new System.Drawing.Size(75, 23);
            this.btnDelGroup.TabIndex = 4;
            this.btnDelGroup.Text = "Delete";
            this.btnDelGroup.UseVisualStyleBackColor = true;
            this.btnDelGroup.Click += new System.EventHandler(this.btnDelGroup_Click);
            // 
            // btnEditGroup
            // 
            this.btnEditGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnEditGroup.Enabled = false;
            this.btnEditGroup.Location = new System.Drawing.Point(97, 447);
            this.btnEditGroup.Name = "btnEditGroup";
            this.btnEditGroup.Size = new System.Drawing.Size(80, 23);
            this.btnEditGroup.TabIndex = 3;
            this.btnEditGroup.Text = "Edit";
            this.btnEditGroup.UseVisualStyleBackColor = true;
            this.btnEditGroup.Click += new System.EventHandler(this.btnEditGroup_Click);
            // 
            // btnAddGroup
            // 
            this.btnAddGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAddGroup.Location = new System.Drawing.Point(11, 447);
            this.btnAddGroup.Name = "btnAddGroup";
            this.btnAddGroup.Size = new System.Drawing.Size(80, 23);
            this.btnAddGroup.TabIndex = 2;
            this.btnAddGroup.Text = "Add";
            this.btnAddGroup.UseVisualStyleBackColor = true;
            this.btnAddGroup.Click += new System.EventHandler(this.btnAddGroup_Click);
            // 
            // btnOkPrivateGroup
            // 
            this.btnOkPrivateGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOkPrivateGroup.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOkPrivateGroup.Enabled = false;
            this.btnOkPrivateGroup.Location = new System.Drawing.Point(713, 447);
            this.btnOkPrivateGroup.Name = "btnOkPrivateGroup";
            this.btnOkPrivateGroup.Size = new System.Drawing.Size(80, 23);
            this.btnOkPrivateGroup.TabIndex = 5;
            this.btnOkPrivateGroup.Text = "OK";
            this.btnOkPrivateGroup.UseVisualStyleBackColor = true;
            this.btnOkPrivateGroup.Click += new System.EventHandler(this.btnOkPrivateGroup_Click);
            // 
            // btnCancelPrivateGroup
            // 
            this.btnCancelPrivateGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancelPrivateGroup.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancelPrivateGroup.Location = new System.Drawing.Point(799, 447);
            this.btnCancelPrivateGroup.Name = "btnCancelPrivateGroup";
            this.btnCancelPrivateGroup.Size = new System.Drawing.Size(80, 23);
            this.btnCancelPrivateGroup.TabIndex = 6;
            this.btnCancelPrivateGroup.Text = "Cancel";
            this.btnCancelPrivateGroup.UseVisualStyleBackColor = true;
            // 
            // contextMenuStripListView
            // 
            this.contextMenuStripListView.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemInfo,
            this.toolStripMenuItemCopy});
            this.contextMenuStripListView.Name = "contextMenuStripListView";
            this.contextMenuStripListView.Size = new System.Drawing.Size(138, 48);
            // 
            // toolStripMenuItemInfo
            // 
            this.toolStripMenuItemInfo.Name = "toolStripMenuItemInfo";
            this.toolStripMenuItemInfo.Size = new System.Drawing.Size(137, 22);
            this.toolStripMenuItemInfo.Text = "Information";
            this.toolStripMenuItemInfo.Click += new System.EventHandler(this.toolStripMenuItemInfo_Click);
            // 
            // toolStripMenuItemCopy
            // 
            this.toolStripMenuItemCopy.Name = "toolStripMenuItemCopy";
            this.toolStripMenuItemCopy.Size = new System.Drawing.Size(137, 22);
            this.toolStripMenuItemCopy.Text = "Copy";
            this.toolStripMenuItemCopy.Click += new System.EventHandler(this.toolStripMenuItemCopy_Click);
            // 
            // SelectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(892, 507);
            this.Controls.Add(this.tabControlList);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MinimizeBox = false;
            this.Name = "SelectForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "RSend Client: Select recipients";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.UsersForm_FormClosing);
            this.Load += new System.EventHandler(this.SelectForm_Load);
            this.Shown += new System.EventHandler(this.SelectForm_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SelectForm_KeyDown);
            this.tabPageADGroup.ResumeLayout(false);
            this.tabControlList.ResumeLayout(false);
            this.tabPageUser.ResumeLayout(false);
            this.tabPageUser.PerformLayout();
            this.tabPageHost.ResumeLayout(false);
            this.tabPageHost.PerformLayout();
            this.tabPagePrivateGroup.ResumeLayout(false);
            this.contextMenuStripListView.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControlList;
        private System.Windows.Forms.TabPage tabPageUser;
        private System.Windows.Forms.ListView listViewUser;
        private System.Windows.Forms.TabPage tabPageHost;
        private System.Windows.Forms.ListView listViewHost;
        private System.Windows.Forms.CheckBox cbSelectAllUser;
        private System.Windows.Forms.Button btnCancelUser;
        private System.Windows.Forms.Button btnOkUser;
        private System.Windows.Forms.CheckBox cbSelectAllHost;
        private System.Windows.Forms.Button btnCancelHost;
        private System.Windows.Forms.Button btnOkHost;
        private System.Windows.Forms.Button btnFilterUser;
        private System.Windows.Forms.ComboBox cbFilterUser;
        private System.Windows.Forms.TextBox txtFilterUser;
        private System.Windows.Forms.Label lblFilterUser;
        private System.Windows.Forms.Button btnFilterHost;
        private System.Windows.Forms.ComboBox cbFilterHost;
        private System.Windows.Forms.TextBox txtFilterHost;
        private System.Windows.Forms.Label lblFilterHost;
        private System.Windows.Forms.Button btnCancelADGroup;
        private System.Windows.Forms.Button btnOkADGroup;
        private System.Windows.Forms.ListView listViewADGroup;
        private System.Windows.Forms.TabPage tabPageADGroup;
        private System.Windows.Forms.TabPage tabPagePrivateGroup;
        private System.Windows.Forms.Button btnOkPrivateGroup;
        private System.Windows.Forms.Button btnCancelPrivateGroup;
        private System.Windows.Forms.Button btnDelGroup;
        private System.Windows.Forms.Button btnEditGroup;
        private System.Windows.Forms.Button btnAddGroup;
        private System.Windows.Forms.ListView listViewPrivateGroup;
        private System.Windows.Forms.Button btnClearUser;
        private System.Windows.Forms.Button btnClearHost;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripListView;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemCopy;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemInfo;
    }
}


