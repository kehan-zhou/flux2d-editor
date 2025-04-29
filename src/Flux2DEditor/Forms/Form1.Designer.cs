using ViewportControl;

namespace Flux2DEditor.Forms
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            viewport1 = new Viewport();
            SuspendLayout();
            // 
            // viewport1
            // 
            viewport1.Dock = DockStyle.Fill;
            viewport1.Location = new Point(0, 47);
            viewport1.Name = "viewport1";
            viewport1.Size = new Size(800, 403);
            viewport1.TabIndex = 0;
            viewport1.Zoom = 1F;
            viewport1.Render += viewport1_Render;
            viewport1.MouseDown += viewport1_MouseDown;
            viewport1.MouseMove += viewport1_MouseMove;
            viewport1.MouseUp += viewport1_MouseUp;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(viewport1);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Viewport viewport1;
    }
}
