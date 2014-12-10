namespace GraphForm.View
{
    partial class MainForm
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
            this.addButton = new System.Windows.Forms.Button();
            this.menuStrip2 = new System.Windows.Forms.MenuStrip();
            this.újGráfToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newGraphItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newUndirected = new System.Windows.Forms.ToolStripMenuItem();
            this.newDirected = new System.Windows.Forms.ToolStripMenuItem();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.linebtn = new System.Windows.Forms.Button();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.StartNodeBox = new System.Windows.Forms.TextBox();
            this.EndNodeBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.edgeWeightBox = new System.Windows.Forms.TextBox();
            this.algorythmChooser = new System.Windows.Forms.ComboBox();
            this.StartBtn = new System.Windows.Forms.Button();
            this.NextBtn = new System.Windows.Forms.Button();
            this.FinishBtn = new System.Windows.Forms.Button();
            this.SpeedBar = new System.Windows.Forms.TrackBar();
            this.menuStrip2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpeedBar)).BeginInit();
            this.SuspendLayout();
            // 
            // addButton
            // 
            this.addButton.Location = new System.Drawing.Point(68, 5);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(75, 20);
            this.addButton.TabIndex = 0;
            this.addButton.Text = "Új csúcs";
            this.addButton.UseVisualStyleBackColor = true;
            // 
            // menuStrip2
            // 
            this.menuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.újGráfToolStripMenuItem});
            this.menuStrip2.Location = new System.Drawing.Point(0, 0);
            this.menuStrip2.Name = "menuStrip2";
            this.menuStrip2.Size = new System.Drawing.Size(1218, 24);
            this.menuStrip2.TabIndex = 2;
            this.menuStrip2.Text = "menuStrip2";
            // 
            // újGráfToolStripMenuItem
            // 
            this.újGráfToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newGraphItem});
            this.újGráfToolStripMenuItem.Name = "újGráfToolStripMenuItem";
            this.újGráfToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.újGráfToolStripMenuItem.Text = "Fájl";
            // 
            // newGraphItem
            // 
            this.newGraphItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newUndirected,
            this.newDirected});
            this.newGraphItem.Name = "newGraphItem";
            this.newGraphItem.Size = new System.Drawing.Size(152, 22);
            this.newGraphItem.Text = "Új gráf";
            // 
            // newUndirected
            // 
            this.newUndirected.Name = "newUndirected";
            this.newUndirected.Size = new System.Drawing.Size(152, 22);
            this.newUndirected.Text = "Irányítatlan";
            // 
            // newDirected
            // 
            this.newDirected.Name = "newDirected";
            this.newDirected.Size = new System.Drawing.Size(152, 22);
            this.newDirected.Text = "Irányított";
            // 
            // linebtn
            // 
            this.linebtn.Location = new System.Drawing.Point(534, 5);
            this.linebtn.Name = "linebtn";
            this.linebtn.Size = new System.Drawing.Size(75, 20);
            this.linebtn.TabIndex = 3;
            this.linebtn.Text = "Hozzáad";
            this.linebtn.UseVisualStyleBackColor = true;
            // 
            // StartNodeBox
            // 
            this.StartNodeBox.Location = new System.Drawing.Point(265, 5);
            this.StartNodeBox.Name = "StartNodeBox";
            this.StartNodeBox.Size = new System.Drawing.Size(50, 20);
            this.StartNodeBox.TabIndex = 4;
            // 
            // EndNodeBox
            // 
            this.EndNodeBox.Location = new System.Drawing.Point(375, 5);
            this.EndNodeBox.Name = "EndNodeBox";
            this.EndNodeBox.Size = new System.Drawing.Size(50, 20);
            this.EndNodeBox.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(170, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Új él: Kezdőcsúcs:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(318, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Végcsúcs:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(436, 7);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "élsúly:";
            // 
            // edgeWeightBox
            // 
            this.edgeWeightBox.Location = new System.Drawing.Point(468, 5);
            this.edgeWeightBox.Name = "edgeWeightBox";
            this.edgeWeightBox.Size = new System.Drawing.Size(50, 20);
            this.edgeWeightBox.TabIndex = 9;
            // 
            // algorythmChooser
            // 
            this.algorythmChooser.FormattingEnabled = true;
            this.algorythmChooser.Location = new System.Drawing.Point(625, 5);
            this.algorythmChooser.Name = "algorythmChooser";
            this.algorythmChooser.Size = new System.Drawing.Size(200, 21);
            this.algorythmChooser.TabIndex = 10;
            // 
            // StartBtn
            // 
            this.StartBtn.Location = new System.Drawing.Point(842, 5);
            this.StartBtn.Name = "StartBtn";
            this.StartBtn.Size = new System.Drawing.Size(75, 20);
            this.StartBtn.TabIndex = 11;
            this.StartBtn.Text = "Start";
            this.StartBtn.UseVisualStyleBackColor = true;
            // 
            // NextBtn
            // 
            this.NextBtn.Location = new System.Drawing.Point(926, 5);
            this.NextBtn.Name = "NextBtn";
            this.NextBtn.Size = new System.Drawing.Size(75, 20);
            this.NextBtn.TabIndex = 12;
            this.NextBtn.Text = "Következő";
            this.NextBtn.UseVisualStyleBackColor = true;
            // 
            // FinishBtn
            // 
            this.FinishBtn.Location = new System.Drawing.Point(1124, 5);
            this.FinishBtn.Name = "FinishBtn";
            this.FinishBtn.Size = new System.Drawing.Size(80, 20);
            this.FinishBtn.TabIndex = 13;
            this.FinishBtn.Text = "Végeredmény";
            this.FinishBtn.UseVisualStyleBackColor = true;
            // 
            // SpeedBar
            // 
            this.SpeedBar.Location = new System.Drawing.Point(1011, 2);
            this.SpeedBar.Maximum = 2000;
            this.SpeedBar.Name = "SpeedBar";
            this.SpeedBar.Size = new System.Drawing.Size(104, 45);
            this.SpeedBar.TabIndex = 14;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1218, 635);
            this.Controls.Add(this.SpeedBar);
            this.Controls.Add(this.FinishBtn);
            this.Controls.Add(this.NextBtn);
            this.Controls.Add(this.StartBtn);
            this.Controls.Add(this.algorythmChooser);
            this.Controls.Add(this.edgeWeightBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.EndNodeBox);
            this.Controls.Add(this.StartNodeBox);
            this.Controls.Add(this.linebtn);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.menuStrip2);
            this.Name = "MainForm";
            this.Text = "Gráfalgoritmusok";
            this.menuStrip2.ResumeLayout(false);
            this.menuStrip2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpeedBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button addButton;
        private System.Windows.Forms.MenuStrip menuStrip2;
        private System.Windows.Forms.ToolStripMenuItem újGráfToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newGraphItem;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Button linebtn;
        private System.Windows.Forms.BindingSource bindingSource1;
        private System.Windows.Forms.TextBox StartNodeBox;
        private System.Windows.Forms.TextBox EndNodeBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox edgeWeightBox;
        private System.Windows.Forms.ComboBox algorythmChooser;
        private System.Windows.Forms.Button StartBtn;
        private System.Windows.Forms.Button NextBtn;
        private System.Windows.Forms.Button FinishBtn;
        private System.Windows.Forms.ToolStripMenuItem newUndirected;
        private System.Windows.Forms.ToolStripMenuItem newDirected;
        private System.Windows.Forms.TrackBar SpeedBar;
    }
}

